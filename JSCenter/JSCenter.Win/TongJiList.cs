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
    public partial class TongJiList : Form
    {
        string projectID = string.Empty;
        public TongJiList(string ID)
        {
            this.projectID = ID;
            InitializeComponent();
        }

        private void TongJiList_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = DAL.CommonDAL.GetTongjiList(projectID);
        }
    }
}
