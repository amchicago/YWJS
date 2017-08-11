using System;
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
            Model.SysConfig config = ReadConfig();
            if (config == null || string.IsNullOrEmpty(config.HanLiang))
            {
                return;
            }
            //计算含量
            Action<Model.DrugProjectItem> countHL=(Model.DrugProjectItem s) => 
            {
                s.HL = "123456";
            };
         
            list.ForEach(s => countHL(s));
            //计算平均含量和方差
            foreach( var item in list.GroupBy(s=>s.CodeNum1))
            {
                double PJHL = 0;
                foreach(var sitem in item)
                {
                    PJHL += Convert.ToInt32(sitem.HL);
                }
            }
            //计算完毕写入数据库

        }
        
        #endregion
    }
}
