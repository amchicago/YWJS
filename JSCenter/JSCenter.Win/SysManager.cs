using System;

namespace JSCenter.Win
{
    public class SysManager
    {

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

    }
}
