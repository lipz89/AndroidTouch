namespace AndroidHelper
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
            this.niIcon.Dispose();
            this.hotkey.Dispose();
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.lblScriptInfo = new System.Windows.Forms.Label();
            this.pnlSet = new System.Windows.Forms.Panel();
            this.btnConnect = new System.Windows.Forms.Button();
            this.rdoWifi = new System.Windows.Forms.RadioButton();
            this.rdoUsb = new System.Windows.Forms.RadioButton();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnParams = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.niIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShutScreen = new System.Windows.Forms.Button();
            this.btnSetShutdownTime = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nudTime = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlSet.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(304, 15);
            this.btnRun.Margin = new System.Windows.Forms.Padding(4);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(81, 29);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "开始(&S)";
            this.btnRun.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(393, 15);
            this.btnPause.Margin = new System.Windows.Forms.Padding(4);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(81, 29);
            this.btnPause.TabIndex = 3;
            this.btnPause.Text = "暂停(&P)";
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(16, 15);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(100, 29);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "任务(&T)...";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // lblInfo1
            // 
            this.lblInfo1.AutoSize = true;
            this.lblInfo1.Location = new System.Drawing.Point(35, 8);
            this.lblInfo1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(71, 15);
            this.lblInfo1.TabIndex = 5;
            this.lblInfo1.Text = "lblInfo1";
            // 
            // lblInfo2
            // 
            this.lblInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo2.Location = new System.Drawing.Point(398, 8);
            this.lblInfo2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(143, 15);
            this.lblInfo2.TabIndex = 6;
            this.lblInfo2.Text = "lblInfo2";
            this.lblInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScriptInfo
            // 
            this.lblScriptInfo.AutoSize = true;
            this.lblScriptInfo.Location = new System.Drawing.Point(124, 21);
            this.lblScriptInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScriptInfo.Name = "lblScriptInfo";
            this.lblScriptInfo.Size = new System.Drawing.Size(79, 15);
            this.lblScriptInfo.TabIndex = 7;
            this.lblScriptInfo.Text = "lblScript";
            // 
            // pnlSet
            // 
            this.pnlSet.Controls.Add(this.label4);
            this.pnlSet.Controls.Add(this.nudTime);
            this.pnlSet.Controls.Add(this.btnSetShutdownTime);
            this.pnlSet.Controls.Add(this.txtIp);
            this.pnlSet.Controls.Add(this.label3);
            this.pnlSet.Controls.Add(this.label2);
            this.pnlSet.Controls.Add(this.btnShutScreen);
            this.pnlSet.Controls.Add(this.btnConnect);
            this.pnlSet.Controls.Add(this.rdoWifi);
            this.pnlSet.Controls.Add(this.rdoUsb);
            this.pnlSet.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSet.Location = new System.Drawing.Point(0, 326);
            this.pnlSet.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSet.Name = "pnlSet";
            this.pnlSet.Size = new System.Drawing.Size(561, 92);
            this.pnlSet.TabIndex = 8;
            this.pnlSet.Visible = false;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(295, 6);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(55, 29);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // rdoWifi
            // 
            this.rdoWifi.AutoSize = true;
            this.rdoWifi.Location = new System.Drawing.Point(12, 11);
            this.rdoWifi.Margin = new System.Windows.Forms.Padding(4);
            this.rdoWifi.Name = "rdoWifi";
            this.rdoWifi.Size = new System.Drawing.Size(60, 19);
            this.rdoWifi.TabIndex = 3;
            this.rdoWifi.TabStop = true;
            this.rdoWifi.Text = "WIFI";
            this.rdoWifi.UseVisualStyleBackColor = true;
            // 
            // rdoUsb
            // 
            this.rdoUsb.AutoSize = true;
            this.rdoUsb.Location = new System.Drawing.Point(221, 11);
            this.rdoUsb.Margin = new System.Windows.Forms.Padding(4);
            this.rdoUsb.Name = "rdoUsb";
            this.rdoUsb.Size = new System.Drawing.Size(52, 19);
            this.rdoUsb.TabIndex = 2;
            this.rdoUsb.TabStop = true;
            this.rdoUsb.Text = "USB";
            this.rdoUsb.UseVisualStyleBackColor = true;
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(71, 8);
            this.txtIp.Margin = new System.Windows.Forms.Padding(4);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(131, 25);
            this.txtIp.TabIndex = 4;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnParams);
            this.pnlMain.Controls.Add(this.listBox1);
            this.pnlMain.Controls.Add(this.btnSelect);
            this.pnlMain.Controls.Add(this.btnRun);
            this.pnlMain.Controls.Add(this.lblScriptInfo);
            this.pnlMain.Controls.Add(this.btnPause);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(4);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(561, 293);
            this.pnlMain.TabIndex = 9;
            // 
            // btnParams
            // 
            this.btnParams.Location = new System.Drawing.Point(483, 15);
            this.btnParams.Margin = new System.Windows.Forms.Padding(4);
            this.btnParams.Name = "btnParams";
            this.btnParams.Size = new System.Drawing.Size(63, 29);
            this.btnParams.TabIndex = 9;
            this.btnParams.Text = "参数";
            this.btnParams.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(16, 51);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(528, 229);
            this.listBox1.TabIndex = 8;
            // 
            // niIcon
            // 
            this.niIcon.Visible = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblInfo1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblInfo2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 293);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(561, 33);
            this.panel1.TabIndex = 11;
            // 
            // btnShutScreen
            // 
            this.btnShutScreen.Location = new System.Drawing.Point(12, 57);
            this.btnShutScreen.Name = "btnShutScreen";
            this.btnShutScreen.Size = new System.Drawing.Size(104, 29);
            this.btnShutScreen.TabIndex = 5;
            this.btnShutScreen.Text = "关闭显示器";
            this.btnShutScreen.UseVisualStyleBackColor = true;
            // 
            // btnSetShutdownTime
            // 
            this.btnSetShutdownTime.Location = new System.Drawing.Point(215, 57);
            this.btnSetShutdownTime.Name = "btnSetShutdownTime";
            this.btnSetShutdownTime.Size = new System.Drawing.Size(104, 29);
            this.btnSetShutdownTime.TabIndex = 6;
            this.btnSetShutdownTime.Text = "定时关机";
            this.btnSetShutdownTime.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(16, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(526, 2);
            this.label2.TabIndex = 7;
            this.label2.Text = "label2";
            // 
            // nudTime
            // 
            this.nudTime.Location = new System.Drawing.Point(135, 59);
            this.nudTime.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTime.Name = "nudTime";
            this.nudTime.Size = new System.Drawing.Size(42, 25);
            this.nudTime.TabIndex = 8;
            this.nudTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudTime.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(178, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "小时";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(325, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "label4";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 418);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlSet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "安卓助手";
            this.pnlSet.ResumeLayout(false);
            this.pnlSet.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime)).EndInit();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.RadioButton rdoWifi;
        private System.Windows.Forms.RadioButton rdoUsb;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnParams;
        private System.Windows.Forms.NotifyIcon niIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown nudTime;
        private System.Windows.Forms.Button btnSetShutdownTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnShutScreen;
        private System.Windows.Forms.Label label4;
    }
}

