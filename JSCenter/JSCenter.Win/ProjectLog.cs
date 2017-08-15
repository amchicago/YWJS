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
    public partial class ProjectLog : Form
    {
        private string _ProjectID;

        public ProjectLog()
        {
            InitializeComponent();
        }

        public ProjectLog(string p)
        {
            // TODO: Complete member initialization
            this._ProjectID = p;
            InitializeComponent();
        }

        private void ProjectLog_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = DAL.CommonDAL.GetLogList(_ProjectID);
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (row == null)
            {
                return;
            }
            LogDetails frm = new LogDetails(row.Cells[0].Value.ToString());
            frm.ShowDialog();
        }
    }
}
