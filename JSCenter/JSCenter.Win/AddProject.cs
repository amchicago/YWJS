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
    public partial class AddProject : Form
    {
        public string ProjectName { get; set; }

        public AddProject()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ProjectName = textBox1.Text.Trim();
            this.Close();
        }

        private void AddProject_Load(object sender, EventArgs e)
        {

        }
    }
}
