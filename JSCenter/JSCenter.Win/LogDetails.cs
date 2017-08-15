using Newtonsoft.Json;
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
            var model = DAL.CommonDAL.GetLogItem(logId);
            label4.Text += model.OpType;
            bool b1 = !string.IsNullOrEmpty(model.BeforeValue);
            bool b2 = !string.IsNullOrEmpty(model.AfterValue);
            Model.DrugProjectItem befor = null;
            Model.DrugProjectItem after = null;
            lblb1.Text="无";
            lblb2.Text="无";
            lblb3.Text="无";
            lblb4.Text="无";
            lblb5.Text = "无";

            lbla1.Text = "无";
            lbla2.Text = "无";
            lbla3.Text = "无";
            lbla4.Text = "无";
            lbla5.Text = "无";
            if (b1)
            {
                befor = JsonConvert.DeserializeObject<Model.DrugProjectItem>(model.BeforeValue);
                lblb1.Text = befor.PJSFMJ;
                lblb2.Text = befor.GSCYL;
                lblb3.Text = befor.XSBS;
                lblb4.Text = befor.DZLD;
                lblb5.Text = befor.DZFMJ;
            }
            if (b2)
            {
                after = JsonConvert.DeserializeObject<Model.DrugProjectItem>(model.AfterValue);
                lbla1.Text = after.PJSFMJ;
                lbla2.Text = after.GSCYL;
                lbla3.Text = after.XSBS;
                lbla4.Text = after.DZLD;
                lbla5.Text = after.DZFMJ;
            }

            if (b1 && b2 && befor != null && after != null)
            {
                if (after.PJSFMJ != befor.PJSFMJ)
                {
                    lblb1.BackColor = Color.Red;
                    lbla1.BackColor = Color.Red;
                }
                if (after.GSCYL != befor.GSCYL)
                {
                    lblb2.BackColor = Color.Red;
                    lbla2.BackColor = Color.Red;
                }
                if (after.XSBS != befor.XSBS)
                {
                    lblb3.BackColor = Color.Red;
                    lbla3.BackColor = Color.Red;
                }
                if (after.DZLD != befor.DZLD)
                {
                    lblb4.BackColor = Color.Red;
                    lbla4.BackColor = Color.Red;
                }
                if (after.DZFMJ != befor.DZFMJ)
                {
                    lblb5.BackColor = Color.Red;
                    lbla5.BackColor = Color.Red;
                }
            }
        }
    }
}
