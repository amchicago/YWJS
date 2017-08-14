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
    public partial class UpdateProjectItem : Form
    {
        public UpdateProjectItem()
        {
            InitializeComponent();
        }

        public string itemID;
        public UpdateProjectItem(string _itemID)
        {
            this.itemID = _itemID;
            InitializeComponent();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Model.DrugProjectItem model = new Model.DrugProjectItem();
            model.ID = Convert.ToInt32(itemID);
            model.PJSFMJ = textBox2.Text.Trim();
            model.GSCYL = textBox3.Text.Trim();
            model.XSBS = textBox4.Text.Trim();
            model.DZLD = textBox5.Text.Trim();
            model.DZFMJ = textBox6.Text.Trim();
            DAL.CommonDAL.UpdateProjectItem(model);
            Common.MessageUtil.ShowTips("修改成功");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void UpdateProjectItem_Load(object sender, EventArgs e)
        {
            var model = DAL.CommonDAL.GetDrugProjectItem(itemID);
            if (model != null)
            {
                txtCode1.Text = model.CodeNum;
                textBox2.Text = model.PJSFMJ;
                textBox3.Text = model.GSCYL;
                textBox4.Text = model.XSBS;
                textBox5.Text = model.DZLD;
                textBox6.Text = model.DZFMJ;
                txtType.Text = model.type;
                txtFuce.Text = model.FuCeStr;
            }

        }
    }
}
