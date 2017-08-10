using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCenter.Model
{
    /// <summary>
    /// 检验数据
    /// </summary>
    public class DrugProjectItem
    {

        public int ID { get; set; }


        public int DrugProjectID { get; set; }


        public string CodeNum
        {
            get
            {
                return CodeNum1 + CodeNum2;
            }
        }

        /// <summary>
        /// 编号 eg.2017-8-7
        /// </summary>
        public string CodeNum1 { get; set; }

        /// <summary>
        /// 编号 eg.供试1
        /// </summary>
        public string CodeNum2 { get; set; }

        /// <summary>
        /// 平均供试峰面积
        /// </summary>
        public string PJSFMJ { get; set; }

        /// <summary>
        /// 供试称样量
        /// </summary>
        public string GSCYL { get; set; }

        /// <summary>
        /// 稀释倍数
        /// </summary>
        public string  XSBS{ get; set; }

        /// <summary>
        /// 对照浓度mg/ml
        /// </summary>
        public string DZLD { get; set; }

        /// <summary>
        /// 对照峰面积
        /// </summary>
        public string DZFMJ { get; set; }

        /// <summary>
        /// 含量
        /// </summary>
        public string HL { get; set; }

        /// <summary>
        /// 平均含量
        /// </summary>
        public string PJHL { get; set; }

        /// <summary>
        /// 方差
        /// </summary>
        public string FC { get; set; }

        /// <summary>
        /// 复测数据：true,不是复测：false
        /// </summary>
        public string IsFuCe { get; set; }

        public string FuCeStr
        {
            get
            {
                return IsFuCe == "True" ? "是" : "否";
            }
        }

    }
}
