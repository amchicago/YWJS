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
    public partial class ConfigCenter : Form
    {
        public ConfigCenter()
        {
            InitializeComponent();
        }

        private void ConfigCenter_Load(object sender, EventArgs e)
        {
            Model.SysConfig model = SysManager.ReadConfig();
            txtHLGS.Text = model.HanLiang;
            txtFCPoint.Text = model.FangChaPoint.ToString();
            txtHLPoint.Text = model.HanLiangPoint.ToString();
            txtPJHLPoint.Text = model.PingJunHLPoint.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Model.SysConfig model = new Model.SysConfig();
            try
            {
                model.HanLiang = txtHLGS.Text;
                model.FangChaPoint = Convert.ToInt32(txtFCPoint.Text);
                model.HanLiangPoint = Convert.ToInt32(txtHLPoint.Text);
                model.PingJunHLPoint = Convert.ToInt32(txtPJHLPoint.Text);
            }catch(Exception)
            {
                Common.MessageUtil.ShowTips("输入格式不正确");
                return;
            }
            SysManager.WriteConfig(model);
            Common.MessageUtil.ShowTips("设置成功！");
        }
    }
}
