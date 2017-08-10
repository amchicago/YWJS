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
        public ProjectLog()
        {
            InitializeComponent();
        }

        private void ProjectLog_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoGenerateColumns = false;
        }
    }
}
