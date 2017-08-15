using Newtonsoft.Json;
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
    [JsonObject(MemberSerialization.OptIn)]
    public class DrugProjectItem
    {
        [JsonIgnoreAttribute]
        public int ID { get; set; }

        [JsonProperty]
        public int DrugProjectID { get; set; }

        [JsonIgnoreAttribute]
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
        [JsonProperty]
        public string CodeNum1 { get; set; }

        /// <summary>
        /// 编号 eg.供试1
        /// </summary>
        [JsonProperty]
        public string CodeNum2 { get; set; }

        /// <summary>
        /// 平均供试峰面积
        /// </summary>
        [JsonProperty]
        public string PJSFMJ { get; set; }

        /// <summary>
        /// 供试称样量
        /// </summary>
        [JsonProperty]
        public string GSCYL { get; set; }

        /// <summary>
        /// 稀释倍数
        /// </summary>
        [JsonProperty]
        public string XSBS { get; set; }

        /// <summary>
        /// 对照浓度mg/ml
        /// </summary>
        [JsonProperty]
        public string DZLD { get; set; }

        /// <summary>
        /// 对照峰面积
        /// </summary>
        [JsonProperty]
        public string DZFMJ { get; set; }

        /// <summary>
        /// 含量
        /// </summary>
        [JsonIgnoreAttribute]
        public string HL { get; set; }

        /// <summary>
        /// 平均含量
        /// </summary>
        [JsonIgnoreAttribute]
        public string PJHL { get; set; }

        /// <summary>
        /// 方差
        /// </summary>
        [JsonIgnoreAttribute]
        public string FC { get; set; }

        /// <summary>
        /// 复测数据：true,不是复测：false
        /// </summary>
        [JsonProperty]
        public string IsFuCe { get; set; }

        [JsonIgnoreAttribute]
        public string FuCeStr
        {
            get
            {
                return IsFuCe == "True" ? "是" : "否";
            }
        }
        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty]
        public string type { get; set; }

    }
}
