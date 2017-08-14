using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq;
namespace JSCenter.Win
{
    public class SysManager
    {
        #region 读写配置
        private const string sysFile = "jsconfig.xml";

        private  static string _GetPath()
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
        public static  void ReCalculation(string projectId)
        {
            var list = DAL.CommonDAL.GetProjectItemList(projectId);
            var fucelist = list.Where(s => s.IsFuCe == "True").ToList();
            var normallist = list.Where(s => s.IsFuCe == "False").ToList();

            #region 加载配置
            Model.SysConfig config = ReadConfig();
            if (config == null || string.IsNullOrEmpty(config.HanLiang))
            {
                return;
            }
            #endregion
            
            # region 计算含量
            Action<Model.DrugProjectItem> countHL=(Model.DrugProjectItem s) => 
            {
                try
                {
                    var dic = new Dictionary<string, FloatingPoint>()
                    {
                         {"平均供试峰面积",float.Parse(s.PJSFMJ)},
                         {"供试称样量",float.Parse(s.GSCYL)},
                         {"稀释倍数",float.Parse(s.XSBS)},
                         {"对照浓度",float.Parse(s.DZLD)},
                         {"对照峰面积",float.Parse(s.DZFMJ)}
                    };
                    FloatingPoint value = Evaluate.Evaluate(dic, Infix.ParseOrUndefined(config.HanLiang));
                    s.HL = Math.Round(value.RealValue, config.HanLiangPoint, MidpointRounding.AwayFromZero).ToString();
                }
                catch (Exception)
                {

                }
            };
            normallist.ForEach(s => countHL(s));
            fucelist.ForEach(s => countHL(s));
            #endregion

            #region 计算平均含量和方差
            
            foreach ( var item in normallist.GroupBy(s=>s.CodeNum1))
            {
                double PJHL = 0;
                int lastId = 0;
                double fangche = 0;
                double lastHL = 0;
                foreach(var sitem in item)
                {
                    PJHL += Convert.ToDouble(sitem.HL);
                    lastId = sitem.ID;
                }
                if (lastId != 0&&item.Count()>0)
                {
                    PJHL = Math.Round(PJHL / item.Count(),config.PingJunHLPoint,MidpointRounding.AwayFromZero);
                    normallist.Where(s => s.ID == lastId).FirstOrDefault().PJHL = PJHL.ToString();
                }
            }
            
            foreach (var item in fucelist.GroupBy(s => s.CodeNum1))
            {
                double PJHL = 0;
                foreach (var sitem in item)
                {
                    PJHL += Convert.ToDouble(sitem.HL);
                }
            }
            #endregion

            //统计数据

            #region  计算完毕更新数据库

            #endregion

        }

        #endregion
    }
}
