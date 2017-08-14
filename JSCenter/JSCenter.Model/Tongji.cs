using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCenter.Model
{
    /// <summary>
    /// 统计信息
    /// </summary>
    public class Tongji
    {
        public string GroupName { get; set; }

        /// <summary>
        /// 汤剂含量
        /// </summary>
        public double TJHL { get; set; }

        /// <summary>
        /// 饮片含量
        /// </summary>
        public double YPHL { get; set; }

        /// <summary>
        /// 转移率
        /// </summary>
        public double ZYLv { get; set; }

    }
}
