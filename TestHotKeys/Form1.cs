using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestHotKeys
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            button1.Click += button1_Click;
            textBox1.KeyDown += textBox1_KeyDown;
            textBox1.KeyUp += textBox1_KeyUp;
            this.FormClosing += Form1_FormClosing;
        }

        readonly HotKeys h = new HotKeys();
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "注册")
            {
                string vaule = textBox1.Text;
                Regist(vaule);
                button1.Text = "卸载";
                label2.Text = "注册成功";
            }
            else
            {
                h.UnRegist(this.Handle);
                button1.Text = "注册";
                label2.Text = "卸载成功";
            }
        }

        private void Regist(string str)
        {
            if (str == "")
                return;
            int modifiers = 0;
            Keys vk = Keys.None;
            foreach (string value in str.Split('+'))
            {
                if (value.Trim() == "Ctrl")
                    modifiers = modifiers + (int)HotKeys.HotkeyModifiers.Control;
                else if (value.Trim() == "Alt")
                    modifiers = modifiers + (int)HotKeys.HotkeyModifiers.Alt;
                else if (value.Trim() == "Shift")
                    modifiers = modifiers + (int)HotKeys.HotkeyModifiers.Shift;
                else
                {
                    if (Regex.IsMatch(value, @"[0-9]"))
                    {
                        vk = (Keys)Enum.Parse(typeof(Keys), "D" + value.Trim());
                    }
                    else
                    {
                        vk = (Keys)Enum.Parse(typeof(Keys), value.Trim());
                    }
                }
            }
            //这里注册了Ctrl+Alt+E 快捷键
            h.Regist(this.Handle, modifiers, vk, CallBack);
        }

        //按下快捷键时被调用的方法
        public void CallBack()
        {
            label2.Text = "快捷键被调用！";
        }

        protected override void WndProc(ref Message m)
        {
            //窗口消息处理函数
            h.ProcessHotKey(m);
            base.WndProc(ref m);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            StringBuilder keyValue = new StringBuilder();
            keyValue.Length = 0;
            keyValue.Append("");
            if (e.Modifiers != 0)
            {
                if (e.Control)
                    keyValue.Append("Ctrl + ");
                if (e.Alt)
                    keyValue.Append("Alt + ");
                if (e.Shift)
                    keyValue.Append("Shift + ");
            }
            if ((e.KeyValue >= 33 && e.KeyValue <= 40) ||
                (e.KeyValue >= 65 && e.KeyValue <= 90) ||   //a-z/A-Z
                (e.KeyValue >= 112 && e.KeyValue <= 123))   //F1-F12
            {
                keyValue.Append(e.KeyCode);
            }
            else if ((e.KeyValue >= 48 && e.KeyValue <= 57))    //0-9
            {
                keyValue.Append(e.KeyCode.ToString().Substring(1));
            }
            ((TextBox)sender).Text = keyValue.ToString();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            string str = ((TextBox)sender).Text.TrimEnd();
            int len = str.Length;
            if (len >= 1 && str.Substring(str.Length - 1) == "+")
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            h.UnRegist(this.Handle);
        }
    }


    class HotKeys
    {
        //引入系统API
        [DllImport("user32.dll")]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, Keys vk);
        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        int keyid = 10; //区分不同的快捷键
        Dictionary<int, HotKeyCallBackHanlder> keymap = new Dictionary<int, HotKeyCallBackHanlder>(); //每一个key对于一个处理函数

        public delegate void HotKeyCallBackHanlder();

        //组合控制键
        public enum HotkeyModifiers
        {
            Alt = 1,
            Control = 2,
            Shift = 4,
            Win = 8
        }

        //注册快捷键
        public void Regist(IntPtr hWnd, int modifiers, Keys vk, HotKeyCallBackHanlder callBack)
        {
            int id = keyid++;
            if (!RegisterHotKey(hWnd, id, modifiers, vk))
                throw new Exception("注册失败！");
            keymap[id] = callBack;
        }

        // 注销快捷键
        public void UnRegist(IntPtr hWnd)
        {
            foreach (KeyValuePair<int, HotKeyCallBackHanlder> var in keymap)
            {
                UnregisterHotKey(hWnd, var.Key);
                return;
            }
        }

        // 快捷键消息处理
        public void ProcessHotKey(Message m)
        {
            if (m.Msg == 0x312)
            {
                int id = m.WParam.ToInt32();
                HotKeyCallBackHanlder callback;
                if (keymap.TryGetValue(id, out callback))
                    callback();
            }
        }
    }
}
