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

        /// <summary>
        /// 操作类型
        /// </summary>
        public string OpType { get; set; }

        public string BeforeValue { get; set; }

        public string AfterValue { get; set; }

        public string ChangeDate { get; set; }
    }

    public enum ProjectLogType
    {
        删除=1,
        修改=2,
        添加=3
    }
}
