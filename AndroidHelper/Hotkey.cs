using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AndroidHelper
{
    public class Hotkey : IMessageFilter, IDisposable
    {
        public delegate void HotkeyEventHandler();
        private readonly IDictionary<uint, HotkeyEventHandler> dictionary = new Dictionary<uint, HotkeyEventHandler>();
        private readonly IntPtr hWnd;

        /// <summary>
        /// 辅助按键
        /// </summary>
        [Flags]
        public enum KeyFlags
        {
            Null = 0x0,
            Alt = 0x1,
            Ctrl = 0x2,
            Shift = 0x4,
            Win = 0x8
        }

        /// <summary>
        /// 注册热键API
        /// </summary>
        [DllImport("user32.dll")]
        private static extern uint RegisterHotKey(IntPtr hWnd, uint id, uint fsModifiers, uint vk);

        /// <summary>
        /// 注销热键API
        /// </summary>
        [DllImport("user32.dll")]
        private static extern uint UnregisterHotKey(IntPtr hWnd, uint id);

        /// <summary>
        /// 全局原子表添加原子
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern uint GlobalAddAtom(string lpString);

        /// <summary>
        /// 全局原子表删除原子
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern uint GlobalDeleteAtom(uint nAtom);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hWnd">当前句柄</param>
        public Hotkey(IntPtr hWnd)
        {
            this.hWnd = hWnd;
            Application.AddMessageFilter(this);
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        public int RegisterHotkey(Keys key, KeyFlags keyflags, HotkeyEventHandler handler)
        {
            var hotkeyid = GlobalAddAtom(Guid.NewGuid().ToString());
            RegisterHotKey(hWnd, hotkeyid, (uint)keyflags, (uint)key);
            dictionary.Add(hotkeyid, handler);
            return (int)hotkeyid;
        }

        /// <summary>
        /// 注销所有热键
        /// </summary>
        public void UnregisterHotkeys()
        {
            Application.RemoveMessageFilter(this);
            foreach (var key in dictionary.Keys)
            {
                UnregisterHotKey(hWnd, key);
                GlobalDeleteAtom(key);
            }
        }

        /// <summary>
        /// 消息筛选
        /// </summary>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x312)
            {
                foreach (var key in dictionary.Keys)
                {
                    if ((uint)m.WParam == key)
                    {
                        if (dictionary.TryGetValue(key, out var callback))
                        {
                            callback();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Dispose()
        {
            UnregisterHotkeys();
        }
    }
}