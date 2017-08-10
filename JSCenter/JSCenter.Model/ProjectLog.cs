using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCenter.Model
{
    public class ProjectLog
    {
        public int ID { get; set; }


        public int  DrugProjectID   { get; set; }

        public int DrugProjectItemID { get; set; }

        /// <summary>
        /// 变更的字段
        /// </summary>
        public string ZiDuan { get; set; }

        public string BeforeValue { get; set; }

        public string AfterValue { get; set; }

        public string ChangeDate { get; set; }
    }
}
