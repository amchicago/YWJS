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
    public partial class LogDetails : Form
    {
        string logId { get; set; }
        public LogDetails(string _id)
        {
            this.logId = _id;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LogDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
