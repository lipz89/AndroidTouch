using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace AndroidHelper
{
    public partial class FrmMain : Form
    {
        private readonly Hotkey hotkey;
        private readonly DriveDetector detector;
        private readonly Regex regexIP = new Regex(@"^((25[0-5]|2[0-4]\d|[1]\d{2}|[1-9]\d|\d)($|(?!\.$)\.)){4}$");

        private const string START_BUTTON_TEXT = "开始(&S)";
        private const string STOP_BUTTON_TEXT = "停止(&S)";
        private const string PAUSE_BUTTON_TEXT = "暂停(&P)";
        private const string CONTINUE_BUTTON_TEXT = "继续(&P)";

        private IScript script = null;

        public FrmMain()
        {
            InitializeComponent();
            hotkey = new Hotkey(this.Handle);
            CheckForIllegalCrossThreadCalls = false;
            detector = new DriveDetector();
            detector.UsbChanged += Detector_UsbChanged;
            btnConnect.Click += BtnConnect_Click;
            lblInfo1.Click += BtnLoad_Click;
            rdoUsb.CheckedChanged += RdoUsb_CheckedChanged;
            rdoWifi.CheckedChanged += RdoUsb_CheckedChanged;
            lblInfo1.Text = "正在初始化...";
            lblScriptInfo.Text = lblInfo2.Text = "未选择脚本...";

            btnRun.Enabled = false;
            btnPause.Enabled = false;
            btnParams.Enabled = false;
            btnSelect.Click += BtnSelect_Click;
            btnRun.Click += BtnRun_Click;
            btnPause.Click += BtnPause_Click;
            btnParams.Click += BtnParams_Click;
            this.Closing += FrmMain_Closing;

            this.Shown += FrmMain_Shown;
            this.niIcon.Icon = this.Icon;
            this.niIcon.Visible = true;
            this.niIcon.DoubleClick += NiIcon_DoubleClick;
            this.RegisterHotKeys();
        }
        private void NiIcon_DoubleClick(object sender, EventArgs e)
        {
            this.ShowOrDisplay();
        }

        private void RegisterHotKeys()
        {
            this.hotkey.RegisterHotkey(Keys.D,
                Hotkey.KeyFlags.Alt | Hotkey.KeyFlags.Ctrl | Hotkey.KeyFlags.Shift,
                this.ShowOrDisplay);
            this.hotkey.RegisterHotkey(Keys.P,
                Hotkey.KeyFlags.Alt | Hotkey.KeyFlags.Ctrl | Hotkey.KeyFlags.Shift,
                this.btnPause.PerformClick);
        }

        private void ShowOrDisplay()
        {
            this.Visible = !this.Visible;
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void BtnParams_Click(object sender, EventArgs e)
        {
            script?.SetParameters();
        }

        private void FrmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.script?.Dispose();
        }

        private void Selector_Selected(object sender, ScriptSelectedArgs e)
        {
            script?.Dispose();

            script = e.Script;
            if (script != null)
            {
                lblScriptInfo.Text = script.Name;// + "  " + script.Desc;
                script.Stopped += Script_Stopped;
                script.NeedParameters += NeedParameters;
                script.CommandRunning += Script_CommandRunning;
                script.CommandRunned += Script_CommandRunned;

                btnRun.Enabled = true;
                btnParams.Enabled = script.HasParameters;
            }
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (!btnPause.Enabled)
            {
                return;
            }
            if (btnPause.Text == PAUSE_BUTTON_TEXT)
            {
                Log("-->暂停");
                script.Pause();
                btnPause.Text = CONTINUE_BUTTON_TEXT;
            }
            else
            {
                Log("-->继续");
                script.Continue();
                btnPause.Text = PAUSE_BUTTON_TEXT;
            }
            SetScriptState();
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            if (btnRun.Text == START_BUTTON_TEXT)
            {
                Log("-->开始");
                script.Start();
                btnRun.Text = STOP_BUTTON_TEXT;
                btnPause.Enabled = true;
                btnSelect.Enabled = false;
            }
            else
            {
                Log("-->停止");
                script.Stop();
                btnRun.Text = START_BUTTON_TEXT;
                btnPause.Text = PAUSE_BUTTON_TEXT;
                btnSelect.Enabled = true;
            }
            SetScriptState();
        }
        internal void NeedParameters(object sender, NeedParameterArgs e)
        {
            var parameters = new FrmParameters();
            parameters.SetParameters(e.Parameters);
            var dlgResult = parameters.ShowDialog(this);
            if (dlgResult != DialogResult.OK)
            {
                e.IsCancel = true;
            }
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            var selector = new FrmSelector();
            selector.Selected += Selector_Selected;
            selector.ShowDialog(this);
        }

        private void Script_CommandRunned(object sender, CommondRunArgs e)
        {
        }

        private void Script_CommandRunning(object sender, CommondRunArgs e)
        {
            this.Log(e.Commond.ToString());
        }

        private void Log(string info)
        {
            lock (listBox1)
            {
                listBox1.Items.Insert(0, info);
                while (listBox1.Items.Count > 100)
                {
                    listBox1.Items.RemoveAt(100);
                }
            }
        }

        private void Script_Stopped(object sender, EventArgs e)
        {
            this.InvokeAction(() =>
            {
                Log("-->全部执行完成");
                btnRun.Text = START_BUTTON_TEXT;
                btnPause.Text = PAUSE_BUTTON_TEXT;
                btnSelect.Enabled = true;
                SetScriptState();
            });
        }

        private void SetScriptState()
        {
            if (script == null)
            {
                lblInfo2.Text = lblScriptInfo.Text = "未选择脚本...";
            }

            var status = script.Context.Status;
            switch (status)
            {
                case Status.Cancelled:
                    lblInfo2.Text = "已中止执行";
                    break;
                case Status.Cancelling:
                    lblInfo2.Text = "正在中止执行";
                    break;
                case Status.Running:
                    lblInfo2.Text = "正在执行...";
                    break;
                case Status.Finished:
                    lblInfo2.Text = "执行结束";
                    break;
                case Status.Inited:
                    lblInfo2.Text = "脚本已加载";
                    break;
                case Status.Paused:
                    lblInfo2.Text = "暂停中...";
                    break;
            }
        }

        private void RdoUsb_CheckedChanged(object sender, EventArgs e)
        {
            txtIp.Enabled = rdoWifi.Checked;
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            pnlMain.Enabled = false;
            this.InvokeAction(() =>
            {
                Global.Init();
                SetState();
                pnlMain.Enabled = true;
            });
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (!Global.IsLoaded)
            {
                Global.LoadAdb();
                SetState();
            }
            else
            {
                pnlSet.Visible = true;
            }
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (rdoWifi.Checked)
            {
                var ip = txtIp.Text.Trim();
                if (!regexIP.IsMatch(ip))
                {
                    MessageBox.Show(this, "IP地址不合法", "提示");
                    txtIp.Focus();
                    return;
                }
                Global.Connect(ip);
            }
            else
            {
                Global.Disconnect();
            }

            SetState();
        }

        private void Detector_UsbChanged(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
            Global.GetMobileInfo();

        }

        private void SetState()
        {
            if (Global.IsConnected)
            {
                var info = Global.Info + " " + Global.MobileSize;
                if (Global.IsWifi)
                {
                    info = $"[{Global.IP}]" + info;
                    rdoWifi.Checked = true;
                    txtIp.Text = Global.IP;
                }
                else
                {
                    info = $"[USB]" + info;
                    rdoUsb.Checked = true;
                }
                lblInfo1.Text = info;
                pnlSet.Visible = false;
            }
            else if (Global.IsLoaded)
            {
                lblInfo1.Text = Global.Info;
            }
            else
            {
                lblInfo1.Text = "未加载ADB文件。";
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == DriveDetector.NativeConstants.WM_DEVICECHANGE)
            {
                if (!Global.IsWifi)
                {
                    bool handled = false;
                    detector.WndProc(m.HWnd, m.Msg, m.WParam, m.LParam, ref handled);
                }
            }

            base.WndProc(ref m);
        }

        private void InvokeAction(ThreadStart action)
        {
            var th = new Thread(() =>
            {
                if (!this.IsDisposed)
                    this.Invoke(action);
            });
            th.Start();
        }
    }
}
