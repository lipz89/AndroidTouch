namespace Androids
{
    partial class Form1
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
            this.lblAdb = new System.Windows.Forms.Label();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.pb = new System.Windows.Forms.PictureBox();
            this.btnPause = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.tbSplit = new System.Windows.Forms.TrackBar();
            this.lblLocation = new System.Windows.Forms.Label();
            this.btnAdb = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblTick = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSplit)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAdb
            // 
            this.lblAdb.AutoSize = true;
            this.lblAdb.Location = new System.Drawing.Point(12, 17);
            this.lblAdb.Name = "lblAdb";
            this.lblAdb.Size = new System.Drawing.Size(107, 12);
            this.lblAdb.TabIndex = 1;
            this.lblAdb.Text = "ADB路径没有配置。";
            // 
            // btnChoose
            // 
            this.btnChoose.Location = new System.Drawing.Point(8, 13);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(75, 23);
            this.btnChoose.TabIndex = 3;
            this.btnChoose.Text = "选择坐标";
            this.btnChoose.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(8, 73);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "开始点击";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // pb
            // 
            this.pb.Dock = System.Windows.Forms.DockStyle.Right;
            this.pb.Location = new System.Drawing.Point(189, 0);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(270, 480);
            this.pb.TabIndex = 5;
            this.pb.TabStop = false;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(8, 244);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 6;
            this.btnPause.Text = "暂停点击";
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "坐标：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "点击次数：";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(89, 146);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(11, 12);
            this.lblCount.TabIndex = 10;
            this.lblCount.Text = "0";
            // 
            // tbSplit
            // 
            this.tbSplit.AutoSize = false;
            this.tbSplit.Location = new System.Drawing.Point(8, 102);
            this.tbSplit.Name = "tbSplit";
            this.tbSplit.Size = new System.Drawing.Size(148, 30);
            this.tbSplit.TabIndex = 11;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(53, 39);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(0, 12);
            this.lblLocation.TabIndex = 12;
            // 
            // btnAdb
            // 
            this.btnAdb.Location = new System.Drawing.Point(115, 12);
            this.btnAdb.Name = "btnAdb";
            this.btnAdb.Size = new System.Drawing.Size(47, 23);
            this.btnAdb.TabIndex = 13;
            this.btnAdb.Text = "...";
            this.btnAdb.UseVisualStyleBackColor = true;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.checkBox1);
            this.pnlMain.Controls.Add(this.lblTick);
            this.pnlMain.Controls.Add(this.btnChoose);
            this.pnlMain.Controls.Add(this.btnStart);
            this.pnlMain.Controls.Add(this.lblLocation);
            this.pnlMain.Controls.Add(this.btnPause);
            this.pnlMain.Controls.Add(this.tbSplit);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.lblCount);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlMain.Location = new System.Drawing.Point(0, 61);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(189, 419);
            this.pnlMain.TabIndex = 14;
            // 
            // lblTick
            // 
            this.lblTick.AutoSize = true;
            this.lblTick.Location = new System.Drawing.Point(89, 78);
            this.lblTick.Name = "lblTick";
            this.lblTick.Size = new System.Drawing.Size(0, 12);
            this.lblTick.TabIndex = 13;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(8, 172);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "忽略宝箱";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 480);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.btnAdb);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.lblAdb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSplit)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAdb;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.PictureBox pb;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.TrackBar tbSplit;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button btnAdb;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblTick;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

