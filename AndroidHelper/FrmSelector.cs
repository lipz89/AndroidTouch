using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AndroidHelper
{
    public partial class FrmSelector : Form
    {
        private const string ROOT = "scripts";
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

            this.Shown += FrmSelector_Shown;
            this.btnOk.Enabled = false;
            this.btnRefresh.Enabled = false;

            this.btnRefresh.Click += BtnRefresh_Click;
            this.btnOk.Click += BtnOk_Click;
            this.btnCancel.Click += BtnCancel_Click;

            this.tvInfos.BeforeSelect += TvInfos_BeforeSelect;
        }

        private void TvInfos_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag == null)
                e.Cancel = true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var node = tvInfos.SelectedNode;
            if (node == null)
            {
                MessageBox.Show(this, "未选择脚本", "提示");
                return;
            }

            var info = node.Tag as ScriptInfo;

            var script = parser.Parse(info.Content, info.Name, info.Desc);
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

        private void RefreshScripts()
        {
            void AddNode(TreeNode node, ScriptInfo info)
            {
                var n = new TreeNode(info.ToString());
                if (!info.IsDir)
                {
                    n.Tag = info;
                    n.ToolTipText = info.Content;
                }
                else
                {
                    n.ForeColor = SystemColors.GrayText;
                    info.Infos?.ForEach(x => AddNode(n, x));
                }

                if (node != null)
                {
                    node.Nodes.Add(n);
                }
                else
                {
                    tvInfos.Nodes.Add(n);
                }
            }
            tvInfos.Nodes.Clear();
            var infos = GetTree();
            infos.ForEach(x => AddNode(null, x));
        }

        private ScriptInfo GetInfo(FileInfo fileInfo)
        {
            using (var r = fileInfo.OpenRead())
            {
                using (var reader = new StreamReader(r))
                {
                    var name = "[未命名]";
                    var desc = string.Empty;
                    var content = reader.ReadToEnd();
                    var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim()).ToList();
                    var nameLine = lines.FirstOrDefault(x => x.StartsWith("@name", StringComparison.OrdinalIgnoreCase));
                    if (nameLine != null)
                    {
                        name = nameLine.Substring(5).Trim();
                    }

                    var descLine = lines.FirstOrDefault(x => x.StartsWith("@desc", StringComparison.OrdinalIgnoreCase));
                    if (descLine != null)
                    {
                        desc = descLine.Substring(5).Trim();
                    }

                    return new ScriptInfo
                    {
                        Path = fileInfo.FullName,
                        Name = name,
                        Desc = desc,
                        Content = content
                    };
                }
            }
        }

        private List<ScriptInfo> GetTree()
        {
            List<ScriptInfo> GetSubTree(DirectoryInfo subDir)
            {
                var subInfos = new List<ScriptInfo>();
                foreach (var directory in subDir.GetDirectories())
                {
                    var folder = new ScriptInfo
                    {
                        IsDir = true,
                        Name = directory.Name,
                        Path = directory.FullName
                    };
                    folder.Infos = GetSubTree(directory);
                    subInfos.Add(folder);
                }
                var subFiles = subDir.GetFiles("*.txt");
                foreach (var fileInfo in subFiles)
                {
                    subInfos.Add(GetInfo(fileInfo));
                }
                return subInfos;
            }

            var dir = new DirectoryInfo(ROOT);
            return GetSubTree(dir);
        }

        class ScriptInfo
        {
            public string Name { get; set; }
            public string Desc { get; set; }
            public string Content { get; set; }
            public string Path { get; set; }
            public bool IsDir { get; set; }
            public List<ScriptInfo> Infos { get; set; }

            public override string ToString()
            {
                if (string.IsNullOrWhiteSpace(Desc))
                    return Name;
                return $"{Name} -- {Desc}";
            }
        }
    }
}
