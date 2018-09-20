using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AndroidHelper
{
    public static class SystemApi
    {
        #region API

        //[DllImport("user32")]
        //public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint msg, uint wParam, int lParam);
        [DllImport("user32")]
        public static extern void LockWorkStation();
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);
        private static readonly IntPtr hwndBroadcast = new IntPtr(0xffff);
        private const uint WM_SYSCOMMAND = 0x0112;
        private const uint SC_MONITORPOWER = 0xf170;

        #endregion
        /// <summary>
        /// 打开显示器
        /// </summary>
        public static void TurnOn()
        {
            SendMessage(hwndBroadcast, WM_SYSCOMMAND, SC_MONITORPOWER, -1);
        }
        /// <summary>
        /// 关闭显示器
        /// </summary>
        public static void TurnOff()
        {
            SendMessage(hwndBroadcast, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
        }

        /// <summary>
        /// 关机
        /// </summary>
        public static void ShutDown()
        {
            Process.Start("shutdown", "/s /t 0"); ;
        }

        private static System.Timers.Timer _timer;
        /// <summary>
        /// 定时关机
        /// </summary>
        /// <param name="seconds">秒</param>
        public static void ShutDownAt(int seconds)
        {
            _timer = new System.Timers.Timer(seconds * 1000) { AutoReset = false };
            _timer.Elapsed += (s, e) => ShutDown();
        }
        /// <summary>
        /// 取消定时关机
        /// </summary>
        public static void CancelShutDown()
        {
            _timer?.Dispose();
        }
        /// <summary>
        /// 重启
        /// </summary>
        public static void Restart()
        {
            Process.Start("shutdown", "/r /t 0");
        }
        /// <summary>
        /// 注销
        /// </summary>
        public static void Logoff()
        {
            //ExitWindowsEx(0, 0);
            Process.Start("logoff");
        }
        /// <summary>
        /// 锁定电脑
        /// </summary>
        public static void Lock()
        {
            LockWorkStation();
        }
        /// <summary>
        /// 休眠
        /// </summary>
        public static void Hibernate()
        {
            SetSuspendState(true, true, true);
        }
        /// <summary>
        /// 睡眠
        /// </summary>
        public static void Sleep()
        {
            SetSuspendState(false, true, true);
        }
    }
}