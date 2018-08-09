using System;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows.Forms;

namespace Androids
{
    public partial class Form1 : Form
    {
        private const string CFG_FILE = "config.txt";
        private const string IMG_PATH = "_.png";
        private string adbPath = "adb.exe";
        private Point point = new Point(0, 0);
        private Size mobileSize = new Size(1080, 1920);
        private AdbRunner adbRunner;
        private bool isLoadAdb = false;
        private bool isAdbSet = false;
        private bool isMobileConnect = false;
        private double picRate = 1;
        private readonly System.Timers.Timer timer;
        private int count = 0;
        private readonly DriveDetector detector;
        private bool isRunning = false;
        private int backTick = 7000;
        private int backTimes = 10, backRealTime = 0;
        private Point boxLocaltion = new Point(520, 1064);
        private Point backLocaltion = new Point(520, 1650);

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            btnAdb.Visible = false;
            pnlMain.Enabled = false;
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            detector = new DriveDetector();
            detector.UsbChanged += Detector_UsbChanged;
            this.Shown += Form1_Load;
            this.btnAdb.Click += BtnAdb_Click;
            this.btnChoose.Click += BtnChoose_Click;
            this.pb.MouseClick += Pb_Click;

            this.btnStart.Click += BtnStart_Click;
            this.btnPause.Click += BtnPause_Click;
            this.tbSplit.ValueChanged += TbSplit_ValueChanged;
            this.tbSplit.Value = 500;
        }

        private void Detector_UsbChanged(object sender, EventArgs e)
        {
            GetMobileInfo();
            ChangeState();
        }

        private void TbSplit_ValueChanged(object sender, EventArgs e)
        {
            lblTick.Text = this.tbSplit.Value + "豪秒";
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            count++;

            var p = this.point;
            if (backRealTime == backTimes)
                p = backLocaltion;
            else if (backRealTime % 2 == 1)
                p = boxLocaltion;

            lblCount.Text = count.ToString();
            listBox1.Items.Insert(0, backRealTime.ToString().PadRight(4) + ":" + p);
            if (listBox1.Items.Count > 100)
            {
                listBox1.Items.RemoveAt(100);
            }
            backRealTime++;
            if (backRealTime > backTimes)
                backRealTime = 0;
            this.adbRunner.Tap(p.X, p.Y);
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            this.timer.Stop();
            isRunning = false;
            ChangeState();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            this.adbRunner.Tap(this.point.X, this.point.Y);
            listBox1.Items.Add("0   :" + this.point.ToString());

            backTimes = backTick / this.tbSplit.Value;
            if (backTimes % 2 == 1)
                backTimes++;
            backRealTime = 1;

            this.timer.Interval = this.tbSplit.Value;
            this.timer.Start();
            isRunning = true;
            ChangeState();
        }

        private void ChangeState()
        {
            this.btnStart.Enabled = !isRunning;
            this.btnChoose.Enabled = !isRunning;
            this.tbSplit.Enabled = !isRunning;
            this.btnPause.Enabled = isRunning;
            if (isRunning)
            {
                if (isMobileConnect)
                {
                    this.timer.Start();
                }
                else
                {
                    this.timer.Stop();
                }
            }
        }

        private void Pb_Click(object sender, MouseEventArgs e)
        {
            if (pb.Image == null || this.timer.Enabled)
                return;

            var x = e.X * picRate;
            var y = e.Y * picRate;
            point = new Point((int)x, (int)y);
            this.lblLocation.Text = point.ToString();
        }

        private void BtnChoose_Click(object sender, EventArgs e)
        {
            SetScreen();
        }

        private void SetScreen()
        {
            pb.Image = null;
            var imagePath = adbRunner.GetShotScreent(IMG_PATH);
            pb.Image = FromFile(imagePath);
        }

        private Image FromFile(string file)
        {
            if (File.Exists(file))
            {
                using (var stream = File.Open(file, FileMode.Open))
                {
                    return Image.FromStream(stream);
                }
            }

            return null;
        }

        private void CreateAdbRunner()
        {
            if (File.Exists(adbPath))
            {
                adbRunner = new AdbRunner(adbPath);
                isLoadAdb = true;
                GetMobileInfo();
            }
            else
            {
                MessageBox.Show("ADB库文件不存在。" + Environment.NewLine + adbPath, "提示");
            }
        }

        private void GetMobileInfo()
        {
            var text = adbRunner.GetMobileInfo();
            if (string.IsNullOrWhiteSpace(text))
            {
                isMobileConnect = false;
                this.Text = "没有连接手机";
            }
            else
            {
                btnAdb.Visible = false;
                isMobileConnect = true;
                this.mobileSize = adbRunner.GetMobileSize();
                this.Text = text + this.mobileSize;

                picRate = this.mobileSize.Height * 1.0 / this.pb.Height;
                var width = this.mobileSize.Width / picRate;
                this.pb.Height = (int)width;

                SetScreen();
            }
        }

        private void BtnAdb_Click(object sender, EventArgs e)
        {
            if (isLoadAdb)
            {
                GetMobileInfo();
            }
            else
            {
                ChooseAdb();
                LoadAdb();
            }
        }

        private void LoadAdb()
        {
            if (!isAdbSet)
            {
                ChooseAdb();
            }
            if (!isAdbSet)
            {
                btnAdb.Visible = true;
                isLoadAdb = false;
                isMobileConnect = false;
            }
            else
            {
                lblAdb.Text = "就绪。";
                pnlMain.Enabled = true;
                CreateAdbRunner();
            }
            ChangeState();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            if (File.Exists(CFG_FILE))
            {
                var path = File.ReadAllText(CFG_FILE);
                if (File.Exists(path) && path.ToLower().EndsWith("exe"))
                {
                    adbPath = path;
                    isAdbSet = true;
                }
            }
            else if (File.Exists("adb.exe"))
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb.exe");
                File.WriteAllText(CFG_FILE, "adb.exe");
                adbPath = path;
                isAdbSet = true;
            }

            LoadAdb();
        }

        private void ChooseAdb()
        {
            var dlg = new OpenFileDialog()
            {
                Filter = "adb库文件|*.exe",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
            var dlr = dlg.ShowDialog(this);
            if (dlr == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    var path = dlg.FileName;
                    File.WriteAllText(CFG_FILE, path);
                    adbPath = path;
                    isAdbSet = true;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            bool handled = false;
            detector.WndProc(m.HWnd, m.Msg, m.WParam, m.LParam, ref handled);
            base.WndProc(ref m);
        }
    }
}
