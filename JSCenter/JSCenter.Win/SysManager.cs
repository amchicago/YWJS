﻿using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq;
namespace JSCenter.Win
{
    public class SysManager
    {
        #region 读写配置
        private const string sysFile = "jsconfig.xml";

        private static string _GetPath()
        {
            return string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, sysFile);
        }
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <returns></returns>
        public static Model.SysConfig ReadConfig()
        {
            string _path = _GetPath();
            if (!System.IO.File.Exists(_path))
            {
                //创建
                WriteConfig(new Model.SysConfig());
            }
            string xml = System.IO.File.ReadAllText(_path);
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }
            return Common.XmlSerializeHelper.DESerializer<Model.SysConfig>(xml);
        }
        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="model"></param>
        public static void WriteConfig(Model.SysConfig model)
        {
            string xml = Common.XmlSerializeHelper.XmlSerialize(model);
            string _path = _GetPath();
            System.IO.File.WriteAllText(_path, xml);
        }
        #endregion

        #region 计算结果

        /// <summary>
        /// 去读配置项重新计算结果,更新数据
        /// </summary>
        public static void ReCalculation(string projectId)
        {
            var list = DAL.CommonDAL.GetProjectItemList(projectId);
            
            var tongjiList = new List<Model.Tongji>();

            #region 加载配置
            Model.SysConfig config = ReadConfig();
            if (config == null || string.IsNullOrEmpty(config.HanLiang))
            {
                return;
            }
            #endregion

            #region 计算含量
            Action<Model.DrugProjectItem> countHL = (Model.DrugProjectItem s) =>
              {
                  try
                  {
                      var dic = new Dictionary<string, FloatingPoint>()
                      {
                         {"供试峰面积",float.Parse(s.PJSFMJ)},
                         {"供试称样量",float.Parse(s.GSCYL)},
                         {"稀释倍数",float.Parse(s.XSBS)},
                         {"对照浓度",float.Parse(s.DZLD)},
                         {"对照峰面积",float.Parse(s.DZFMJ)}
                      };
                      FloatingPoint value = Evaluate.Evaluate(dic, Infix.ParseOrUndefined(config.HanLiang));
                      s.HL = Math.Round(value.RealValue, config.HanLiangPoint, MidpointRounding.AwayFromZero).ToString();
                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine(ex.ToString());
                  }
              };
            #endregion

            #region 计算平均含量和方差

            list.ForEach(s=>countHL(s));

            var yplist = list.Where(s => s.type.Trim() == Model.DrugType.饮片.ToString());
            var tjlist = list.Where(s => s.type.Trim() == Model.DrugType.汤剂.ToString());
           
            Action<string ,double,bool> addTongji = (string key, double PJHL,bool isYp) =>
              {
                  if (isYp)
                  {
                      if (tongjiList.Any(s => s.GroupName == key))
                      {
                          tongjiList.Where(s => s.GroupName == key).FirstOrDefault().YPHL = PJHL;
                      }
                      else
                      {
                          tongjiList.Add(new Model.Tongji() { GroupName = key, YPHL = PJHL });
                      }
                  }
                  else
                  {
                      if (tongjiList.Any(s => s.GroupName == key))
                      {
                          tongjiList.Where(s => s.GroupName == key).FirstOrDefault().TJHL = PJHL;
                      }
                      else
                      {
                          tongjiList.Add(new Model.Tongji() { GroupName = key, TJHL = PJHL });
                      }
                  }
                  
              };

            Action<List<Model.DrugProjectItem>,bool> countPJHL = (List<Model.DrugProjectItem> models,bool fc) =>
            {
                foreach (var item in models.GroupBy(s => s.CodeNum1))
                {
                    double PJHL = 0;
                    int lastId = 0;
                    List<FcEntity> fclist = new List<Win.FcEntity>();
                    int index = 0;
                    foreach (var sitem in item)
                    {
                        PJHL += Convert.ToDouble(sitem.HL);
                        lastId = sitem.ID;
                        if (index > 1)
                        {
                            index = 0;
                        }
                        fclist.Add(new Win.FcEntity() { HL = Convert.ToDouble(sitem.HL), ID = sitem.ID });
                    }
                    if (lastId != 0 && item.Count() > 1)
                    {
                        PJHL = Math.Round(PJHL / item.Count(), config.PingJunHLPoint, MidpointRounding.AwayFromZero);
                        list.Where(s => s.ID == lastId).FirstOrDefault().PJHL = PJHL.ToString();
                        addTongji(item.Key, PJHL, fc);
                        if (fc)
                        {
                            if (fclist.Count >= 2)
                            {
                                double fac = 0;
                                fclist = fclist.OrderBy(s => s.ID).ToList();
                                for (int i = 1; i < fclist.Count; i++)
                                {
                                    fac = fclist[i-1].HL - fclist[i].HL;
                                }
                                list.Where(s => s.ID == lastId).FirstOrDefault().FC = Math.Round(fac,config.FangChaPoint ,MidpointRounding.AwayFromZero).ToString();
                            }
                        }
                    }
                }
            };

            countPJHL(yplist.Where(s => s.IsFuCe == "False").ToList(),true);
            countPJHL(yplist.Where(s => s.IsFuCe != "False").ToList(),true);

            countPJHL(tjlist.Where(s => s.IsFuCe == "False").ToList(),false);
            countPJHL(tjlist.Where(s => s.IsFuCe != "False").ToList(),false);

            #endregion

            #region 统计数据
            foreach( var item in tongjiList)
            {
                item.ZYLv = Math.Round((item.TJHL / item.YPHL) * 100, 2, MidpointRounding.AwayFromZero); 
            }
            DAL.CommonDAL.AddProjectTongji(tongjiList, projectId);
            #endregion

            #region  计算完毕更新数据库

            foreach ( var item in list)
            {
                DAL.CommonDAL.UpdateProjectItem(item);
            }
            #endregion

        }

        #endregion
    }


    public class FcEntity
    {
        public double HL { get; set; }

        public int ID { get; set; }
    }

}
