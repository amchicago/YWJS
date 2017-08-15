using JSCenter.Common;
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
             DataGridViewRow row = dataGridView1.CurrentRow;
            if (row == null)
            {
                return;
            }
            UpdateProjectItem frm = new UpdateProjectItem(row.Cells[0].Value.ToString());
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void 查看统计数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TongJiList frm = new TongJiList(_ProjectID);
            frm.ShowDialog();
        }


        void LoadData()
        {
            SysManager.ReCalculation(_ProjectID);

            var list= DAL.CommonDAL.GetProjectItemList(_ProjectID).OrderBy(s=>s.ID).ToList();
            this.dataGridView1.DataSource = list;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (MessageUtil.ConfirmYesNo(string.Format("确认要删除{0}？", row.Cells[1].Value.ToString())))
            {
                DAL.CommonDAL.DeleteProjectItem(row.Cells[0].Value.ToString());
                LoadData();
            }
        }
    }
}
