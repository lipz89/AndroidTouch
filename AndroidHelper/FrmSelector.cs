using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AndroidHelper
{
    public partial class FrmSelector : Form
    {
        private const string ROOT = "scripts";
        private ScriptInfo info = null;
        private Label last = null;
        private readonly Parser parser = new Parser();
        private event EventHandler<ScriptSelectedArgs> selected;
        internal event EventHandler<ScriptSelectedArgs> Selected
        {
            add => selected += value;
            remove => selected -= value;
        }

        public FrmSelector()
        {
            InitializeComponent();

            //this.parser.NeedParameters += Parser_NeedParameters;
            this.Shown += FrmSelector_Shown;
            this.btnOk.Enabled = false;
            this.btnRefresh.Enabled = false;

            this.btnRefresh.Click += BtnRefresh_Click;
            this.btnOk.Click += BtnOk_Click;
            this.btnCancel.Click += BtnCancel_Click;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (info == null)
            {
                MessageBox.Show(this, "未选择脚本", "提示");
                return;
            }

            var script = parser.Parse(info.Content);
            if (script != null)
            {
                selected?.Invoke(this, new ScriptSelectedArgs { Script = script });
                this.Close();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.btnRefresh.Enabled = false;
            this.btnOk.Enabled = false;
            RefreshScripts();
            this.btnRefresh.Enabled = true;
            this.btnOk.Enabled = true;
            this.btnOk.Focus();
        }

        private void FrmSelector_Shown(object sender, EventArgs e)
        {
            RefreshScripts();
            this.btnRefresh.Enabled = true;
            this.btnOk.Enabled = true;
            this.btnOk.Focus();
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            if (sender is Label lbl)
            {
                if (lbl != last)
                {
                    if (last != null)
                    {
                        last.BackColor = this.BackColor;
                        last.ForeColor = this.ForeColor;
                    }

                    lbl.BackColor = Color.CornflowerBlue;
                    lbl.ForeColor = Color.White;
                    last = lbl;
                    info = lbl.Tag as ScriptInfo;
                }
            }
        }

        private void RefreshScripts()
        {
            var infos = GetAll();
            int top = 10, left = 10;
            foreach (var info in infos)
            {
                var lbl = new Label()
                {
                    Text = info.Name,
                    AutoSize = false,
                    Width = 150,
                    Height = 20,
                    BorderStyle = BorderStyle.FixedSingle,
                    Left = left,
                    Top = top,
                    Tag = info,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                lbl.Click += Lbl_Click;
                pnlScripts.Controls.Add(lbl);
                if (left + 310 > pnlScripts.Width)
                {
                    left = 10;
                    top += 40;
                }
                else
                {
                    left += 210;
                }
            }

            pnlScripts.AutoScroll = true;
            if (last != null)
            {
                last.BackColor = this.BackColor;
                last.ForeColor = this.ForeColor;
            }

            info = null;
            last = null;
        }

        private List<ScriptInfo> GetAll()
        {
            var infos = new List<ScriptInfo>();
            var dir = new DirectoryInfo(ROOT);
            var files = dir.GetFiles("*.txt", SearchOption.AllDirectories);
            foreach (var fileInfo in files)
            {
                using (var r = fileInfo.OpenRead())
                {
                    using (var reader = new StreamReader(r))
                    {
                        var name = "[未命名]";
                        var content = reader.ReadToEnd();
                        var lines = content.Split(new[] { '\r', '\n' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        if (lines[0].StartsWith("@name", StringComparison.OrdinalIgnoreCase))
                        {
                            name = lines[0].Substring(5).Trim();
                        }
                        infos.Add(new ScriptInfo
                        {
                            Path = fileInfo.FullName,
                            Name = name,
                            Content = content
                        });
                    }
                }
            }

            return infos;
        }

        class ScriptInfo
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public string Path { get; set; }
        }
    }
    class ScriptSelectedArgs : EventArgs
    {
        public IScript Script { get; set; }
    }
}
