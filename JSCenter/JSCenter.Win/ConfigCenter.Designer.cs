namespace JSCenter.Win
{
    partial class ConfigCenter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.txtHLGS = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHLPoint = new System.Windows.Forms.TextBox();
            this.txtPJHLPoint = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFCPoint = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(207, 351);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "应 用";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtHLGS
            // 
            this.txtHLGS.Location = new System.Drawing.Point(138, 42);
            this.txtHLGS.Multiline = true;
            this.txtHLGS.Name = "txtHLGS";
            this.txtHLGS.Size = new System.Drawing.Size(328, 87);
            this.txtHLGS.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "含量计算公式";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 194);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "含量保留小数位";
            // 
            // txtHLPoint
            // 
            this.txtHLPoint.Location = new System.Drawing.Point(148, 191);
            this.txtHLPoint.Name = "txtHLPoint";
            this.txtHLPoint.Size = new System.Drawing.Size(68, 21);
            this.txtHLPoint.TabIndex = 4;
            // 
            // txtPJHLPoint
            // 
            this.txtPJHLPoint.Location = new System.Drawing.Point(148, 241);
            this.txtPJHLPoint.Name = "txtPJHLPoint";
            this.txtPJHLPoint.Size = new System.Drawing.Size(68, 21);
            this.txtPJHLPoint.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 244);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "平均含量保留小数位";
            // 
            // txtFCPoint
            // 
            this.txtFCPoint.Location = new System.Drawing.Point(148, 293);
            this.txtFCPoint.Name = "txtFCPoint";
            this.txtFCPoint.Size = new System.Drawing.Size(68, 21);
            this.txtFCPoint.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 296);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "方差保留小数位";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(290, 301);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "(注意：默认采用四舍五入)";
            // 
            // ConfigCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 422);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtFCPoint);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPJHLPoint);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtHLPoint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHLGS);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigCenter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统配置";
            this.Load += new System.EventHandler(this.ConfigCenter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtHLGS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPJHLPoint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFCPoint;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtHLPoint;
        private System.Windows.Forms.Label label5;
    }
}