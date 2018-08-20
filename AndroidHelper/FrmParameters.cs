using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AndroidHelper
{
    public partial class FrmParameters : Form
    {
        private UcParameter last = null;
        private UcParameter current = null;
        private double pbRate = 1;
        public FrmParameters()
        {
            InitializeComponent();
            this.pnlParameters.AutoScroll = true;

            this.btnCancel.Click += BtnCancel_Click;
            this.btnReset.Click += BtnReset_Click;
            this.btnOk.Click += BtnOk_Click;
            this.btnScreen.Click += BtnScreen_Click;
            this.pbScreen.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbScreen.MouseClick += PbScreen_Click;
            this.Shown += FrmParameters_Shown;
        }

        private void FrmParameters_Shown(object sender, EventArgs e)
        {
            pbRate = Global.MobileSize.Height * 1.0 / this.pbScreen.Height;
            this.pbScreen.Width = (int)(Global.MobileSize.Width / pbRate);
            if (File.Exists(AdbRunner.IMG_PATH))
            {
                using (var stream = File.Open(AdbRunner.IMG_PATH, FileMode.Open))
                {
                    pbScreen.Image = Image.FromStream(stream);
                }
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var flag = true;
            foreach (var ucParameter in pnlParameters.Controls.OfType<UcParameter>())
            {
                ucParameter.Apply();
                flag &= ucParameter.HasValue();
            }

            if (flag)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            foreach (var ucParameter in pnlParameters.Controls.OfType<UcParameter>())
            {
                ucParameter.Reset();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        internal void SetParameters(List<IParameter> parameters)
        {
            this.pnlParameters.Controls.Clear();
            foreach (var parameter in parameters)
            {
                var up = new UcParameter();
                up.Parameter = parameter;
                up.Dock = DockStyle.Top;
                up.GotFocus += Up_GotFocus;
                this.pnlParameters.Controls.Add(up);
            }
        }

        private void Up_GotFocus(object sender, EventArgs e)
        {
            if (sender is UcParameter up)
            {
                if (up != last)
                {
                    last?.LostUserFocus();
                    last = up;
                    up.GetUserFocus();
                    current = up;
                }
            }
        }
        private void BtnScreen_Click(object sender, EventArgs e)
        {
            if (Global.IsConnected)
            {
                this.pbScreen.Image = Global.Runner.GetShotScreent();
            }
        }

        private void PbScreen_Click(object sender, MouseEventArgs e)
        {
            if (pbScreen.Image == null || current == null || current.ParameterType != ParameterType.Point)
                return;

            var x = e.X * pbRate;
            var y = e.Y * pbRate;
            var point = new Point { X = (int)x, Y = (int)y };
            current.SetValue(point);
        }
    }
}
