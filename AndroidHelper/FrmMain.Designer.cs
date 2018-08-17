﻿namespace AndroidHelper
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRun = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.lblScriptInfo = new System.Windows.Forms.Label();
            this.pnlSet = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.rdoUsb = new System.Windows.Forms.RadioButton();
            this.rdoWifi = new System.Windows.Forms.RadioButton();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.pnlSet.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(253, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "开始(&S)";
            this.btnRun.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(334, 12);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 3;
            this.btnPause.Text = "暂停(&P)";
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(12, 12);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "任务(&T)...";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // lblInfo1
            // 
            this.lblInfo1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInfo1.AutoSize = true;
            this.lblInfo1.Location = new System.Drawing.Point(10, 215);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(53, 12);
            this.lblInfo1.TabIndex = 5;
            this.lblInfo1.Text = "lblInfo1";
            // 
            // lblInfo2
            // 
            this.lblInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo2.Location = new System.Drawing.Point(302, 215);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(107, 12);
            this.lblInfo2.TabIndex = 6;
            this.lblInfo2.Text = "lblInfo2";
            this.lblInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScriptInfo
            // 
            this.lblScriptInfo.AutoSize = true;
            this.lblScriptInfo.Location = new System.Drawing.Point(93, 17);
            this.lblScriptInfo.Name = "lblScriptInfo";
            this.lblScriptInfo.Size = new System.Drawing.Size(59, 12);
            this.lblScriptInfo.TabIndex = 7;
            this.lblScriptInfo.Text = "lblScript";
            // 
            // pnlSet
            // 
            this.pnlSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlSet.Controls.Add(this.txtIp);
            this.pnlSet.Controls.Add(this.rdoWifi);
            this.pnlSet.Controls.Add(this.rdoUsb);
            this.pnlSet.Controls.Add(this.btnConnect);
            this.pnlSet.Controls.Add(this.btnLoad);
            this.pnlSet.Location = new System.Drawing.Point(69, 215);
            this.pnlSet.Name = "pnlSet";
            this.pnlSet.Size = new System.Drawing.Size(200, 56);
            this.pnlSet.TabIndex = 8;
            this.pnlSet.Visible = false;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(3, 30);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "选择ADB";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Visible = false;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(122, 30);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "连接手机";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Visible = false;
            // 
            // rdoUsb
            // 
            this.rdoUsb.AutoSize = true;
            this.rdoUsb.Location = new System.Drawing.Point(3, 8);
            this.rdoUsb.Name = "rdoUsb";
            this.rdoUsb.Size = new System.Drawing.Size(41, 16);
            this.rdoUsb.TabIndex = 2;
            this.rdoUsb.TabStop = true;
            this.rdoUsb.Text = "USB";
            this.rdoUsb.UseVisualStyleBackColor = true;
            this.rdoUsb.Visible = false;
            // 
            // rdoWifi
            // 
            this.rdoWifi.AutoSize = true;
            this.rdoWifi.Location = new System.Drawing.Point(50, 8);
            this.rdoWifi.Name = "rdoWifi";
            this.rdoWifi.Size = new System.Drawing.Size(47, 16);
            this.rdoWifi.TabIndex = 3;
            this.rdoWifi.TabStop = true;
            this.rdoWifi.Text = "WIFI";
            this.rdoWifi.UseVisualStyleBackColor = true;
            this.rdoWifi.Visible = false;
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(98, 6);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(99, 21);
            this.txtIp.TabIndex = 4;
            this.txtIp.Visible = false;
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMain.Controls.Add(this.listBox1);
            this.pnlMain.Controls.Add(this.btnSelect);
            this.pnlMain.Controls.Add(this.btnRun);
            this.pnlMain.Controls.Add(this.lblScriptInfo);
            this.pnlMain.Controls.Add(this.btnPause);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(421, 210);
            this.pnlMain.TabIndex = 9;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 41);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(397, 160);
            this.listBox1.TabIndex = 8;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 236);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSet);
            this.Controls.Add(this.lblInfo2);
            this.Controls.Add(this.lblInfo1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "安卓助手";
            this.pnlSet.ResumeLayout(false);
            this.pnlSet.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label lblInfo1;
        private System.Windows.Forms.Label lblInfo2;
        private System.Windows.Forms.Label lblScriptInfo;
        private System.Windows.Forms.Panel pnlSet;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.RadioButton rdoWifi;
        private System.Windows.Forms.RadioButton rdoUsb;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ListBox listBox1;
    }
}
