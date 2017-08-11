using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace JSCenter.Win
{
    public partial class ProjectList : Form
    {

        public string _ProjectID { get; set; }
        public ProjectList()
        {
            InitializeComponent();
        }

        public ProjectList(string projectID)
        {
            this._ProjectID = projectID;
            InitializeComponent();
        }

        private void ProjectList_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;

            SysManager.ReCalculation(_ProjectID);
            LoadData();
        }
        private void 添加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var add = new AddProjectItem(_ProjectID);
            add.OnDataInsert += add_OnDataInsert;
            add.ShowDialog();
        }

        void add_OnDataInsert()
        {
            LoadData();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 查看统计数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        void LoadData()
        {
             
            var list= DAL.CommonDAL.GetProjectItemList(_ProjectID).OrderBy(s => s.IsFuCe).ToList();
            
            this.dataGridView1.DataSource = list;
        }
       
    }
}
