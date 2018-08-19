using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AndroidHelper
{
    static class Global
    {
        private const string CFG_FILE = "config.txt";
        private const string ADB_FILE_NAME = "adb.exe";
        private static string adbPath = "adb.exe";

        public static AdbRunner Runner { get; private set; }
        public static bool IsLoaded { get; private set; }
        private static bool IsAdbSetted { get; set; }
        public static bool IsConnected { get; private set; }
        public static bool IsWifi { get; private set; }
        public static string Info { get; private set; }
        public static Size MobileSize { get; private set; }
        public static string IP { get; private set; }

        public static void Init()
        {
            if (File.Exists(CFG_FILE))
            {
                var path = File.ReadAllText(CFG_FILE);
                if (File.Exists(path) && path.ToLower().EndsWith("exe"))
                {
                    adbPath = path;
                    IsAdbSetted = true;
                }
            }
            else if (File.Exists(ADB_FILE_NAME))
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ADB_FILE_NAME);
                File.WriteAllText(CFG_FILE, ADB_FILE_NAME);
                adbPath = path;
                IsAdbSetted = true;
            }

            LoadAdb();
        }
        public static void LoadAdb()
        {
            if (!IsAdbSetted || !File.Exists(adbPath))
            {
                ChooseAdb();
            }
            if (IsAdbSetted)
            {
                Runner = new AdbRunner(adbPath);
                IsLoaded = true;
                GetMobileInfo();
            }
        }
        private static void ChooseAdb()
        {
            var dlg = new OpenFileDialog()
            {
                Filter = @"adb库文件|*.exe",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
            var dlr = dlg.ShowDialog();
            if (dlr == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    var path = dlg.FileName;
                    File.WriteAllText(CFG_FILE, path);
                    adbPath = path;
                    IsAdbSetted = true;
                }
            }
        }
        public static void GetMobileInfo()
        {
            var flag = Runner.GetMobileInfo(out string message, out string ip);
            Info = message;
            IsConnected = flag;
            if (IsConnected)
            {
                if (!string.IsNullOrWhiteSpace(ip))
                {
                    IsWifi = true;
                    IP = ip;
                }
                else
                {
                    IsWifi = false;
                }

                var size = Runner.GetMobileSize();
                if (size.HasValue)
                {
                    MobileSize = size.Value;
                }
            }
        }
        public static void Disconnect()
        {
            Runner.Disconnect();
            GetMobileInfo();
        }
        public static void Connect(string ip)
        {
            var flag = Runner.Connect(ip, 5555, out var message);
            if (flag)
            {
                GetMobileInfo();
            }
            else
            {
                Info = message;
            }
        }
    }
}