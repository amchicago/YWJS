﻿using System;
using System.Windows.Forms;

namespace JSCenter.Win
{
    public partial class AddProjectItem : Form
    {
        public delegate void InsertDataHandler();

        public event InsertDataHandler OnDataInsert;

        private string _ProjectID;

        public AddProjectItem()
        {
            InitializeComponent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_ProjectID"></param>
        /// <param name="ID">ID</param>
        public AddProjectItem(string _ProjectID)
        {
            this._ProjectID = _ProjectID;
            InitializeComponent();
        }

        private void AddProjectItem_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = System.Enum.GetNames(typeof(Model.DrugType));
            Model.DrugType yp = Model.DrugType.饮片;
            comboBox1.SelectedIndex = this.comboBox1.FindString(yp.ToString());
            SetTxtBox();
        }

        private void SetTxtBox()
        {
            txtCode1.Text = string.Format("{0}-{1}-{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar >= 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                e.Handled = true;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Model.DrugProjectItem model = new Model.DrugProjectItem();
            model.CodeNum1 = txtCode1.Text.Trim();
            model.CodeNum2 = txtCode2.Text.Trim();
            model.PJSFMJ = textBox2.Text.Trim();
            model.GSCYL = textBox3.Text.Trim();
            model.XSBS = textBox4.Text.Trim();
            model.DZLD = textBox5.Text.Trim();
            model.DZFMJ = textBox6.Text.Trim();
            model.IsFuCe = checkBox1.Checked.ToString();
            model.DrugProjectID = Convert.ToInt32( _ProjectID);
            model.type = comboBox1.SelectedItem.ToString().Trim();
            if (ValidateModel(model))
            {
                DAL.CommonDAL.AddProjectItem(model);
                this.OnDataInsert();
            }
            else
            {
                Common.MessageUtil.ShowTips("请填写完整信息");
            }
        }

        bool ValidateModel(Model.DrugProjectItem model)
        {
            if (string.IsNullOrEmpty(model.CodeNum1))
            {
                return false;
            }
            if (string.IsNullOrEmpty(model.PJSFMJ))
            {
                return false;
            }
            if (string.IsNullOrEmpty(model.GSCYL))
            {
                return false;
            }
            if (string.IsNullOrEmpty(model.XSBS))
            {
                return false;
            }
            if (string.IsNullOrEmpty(model.DZLD))
            {
                return false;
            }
            if (string.IsNullOrEmpty(model.DZFMJ))
            {
                return false;
            }
            return true;
        }
    }
}
