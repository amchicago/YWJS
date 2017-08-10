using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCenter.Model
{

   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SysConfig
    {
        /// <summary>
        /// 含量计算公式
        /// </summary>
        public string HanLiang { get; set; }

        /// <summary>
        /// 含量保留几位小数
        /// </summary>
        public int HanLiangPoint { get; set; }

        /// <summary>
        /// 平均含量保留几位小数
        /// </summary>
        public int PingJunHLPoint { get; set; }

        /// <summary>
        /// 方差保留几位小数
        /// </summary>
        public int FangChaPoint { get; set; }
    }
}
