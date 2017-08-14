using JSCenter.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSCenter.Win
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void 药材检测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProject frm = new AddProject();
            frm.ShowDialog();
            if (!string.IsNullOrEmpty(frm.ProjectName))
            {
                DAL.CommonDAL.AddProject(new Model.DrugProject() { DurgName=frm.ProjectName});
                LoadData();
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigCenter cc = new ConfigCenter();
            cc.ShowDialog();
        }

        private void 详细数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (row == null)
            {
                return;
            }
            ProjectList list = new ProjectList(row.Cells[0].Value.ToString());
            list.ShowDialog();
        }

        private void 日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            ProjectLog log = new ProjectLog();
            log.ShowDialog();
        }

        private void 导出ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataGridViewRow row = dataGridView1.CurrentRow;
            if (row == null)
            {
                return;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ExcelManager.WriteExcel(row.Cells[0].Value.ToString(), saveFileDialog1.SelectedPath);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (MessageUtil.ConfirmYesNo(string.Format("确认要删除{0}？", row.Cells[1].Value.ToString())))
            {
                DAL.CommonDAL.DeleteProject(row.Cells[0].Value.ToString());
                LoadData();
            }
        } 

        void LoadData()
        {
            this.dataGridView1.DataSource = DAL.CommonDAL.GetProjectList();
        }

        
    }
}
