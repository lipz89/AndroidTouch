using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AndroidHelper
{
    internal class AdbRunner
    {
        private readonly string adbPath;
        private const string IMG_MOBILE_PATH = "/sdcard/_1.png";
        private const string IMG_PATH = "_.png";

        public AdbRunner(string adbPath)
        {
            this.adbPath = adbPath;
        }

        private string RunAdb(string cmd, bool withResult = true)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = adbPath;
                process.StartInfo.Arguments = cmd;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                if (withResult)
                {
                    var text = process.StandardOutput.ReadToEndAsync().Result;
                    return text.Trim();
                }
                else
                {
                    return null;
                }
            }
        }

        private void RunWithoutResult(string command)
        {
            RunAdb(command, false);
        }
        private string Run(string command)
        {
            return RunAdb(command, true);
        }

        private IList<string> GetLines(string txt)
        {
            return txt.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public bool IsTheGame()
        {
            var txt = Run("shell \"dumpsys activity | grep mFocusedActivity\"");
            return txt.Contains("com.zn.monster");
        }
        public bool IsLockedOrPowerOff()
        {
            var txt = Run("shell \"dumpsys window policy | grep mShowingLockscreen\"");
            if (txt.Contains("mShowingLockscreen=true"))
            {
                return true;
            }
            txt = Run("shell \"dumpsys power | grep 'Display Power'\"");
            if (txt.Contains("state=OFF"))
            {
                return true;
            }
            return false;
        }
        public bool GetMobileInfo(out string message, out string ip)
        {
            ip = "";
            var txt = Run("devices");
            var lines = txt.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
            if (lines.Any())
            {
                var line = lines.FirstOrDefault(x => x.Contains(".") && x.Contains(":"));
                if (line != null)
                {
                    var index = line.IndexOf(":", StringComparison.Ordinal);
                    ip = line.Substring(0, index);
                }
                if (lines.All(x => x.EndsWith("offline")))
                {
                    message = "设备不在线。";
                    return false;
                }
                message = Run("shell getprop ro.product.model");
                return true;
            }

            message = "未连接到手机";
            return false;
        }

        public bool Connect(string ip, uint port, out string message)
        {
            message = "";
            var txt = Run("tcpip " + port);
            if (txt.StartsWith("error"))
            {
                message = txt;
                return false;
            }

            txt = Run("connect " + ip);
            if (!txt.Contains("connected"))
            {
                message = txt;
                return false;
            }
            return true;
        }

        private readonly Regex sizeReg = new Regex(@"\d+x\d+");
        public Size GetMobileSize()
        {
            var text = Run("shell \"dumpsys window | grep mUnrestrictedScreen\"");
            var match = sizeReg.Match(text);
            var size = match.Value;
            var sizes = size.Split('x');
            return new Size(int.Parse(sizes[0]), int.Parse(sizes[1]));
        }
        public Image GetShotScreent()
        {
            var path = IMG_PATH;
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Run($"shell screencap -p {IMG_MOBILE_PATH}");
            Run($"pull {IMG_MOBILE_PATH} {path}");

            if (File.Exists(path))
            {
                using (var stream = File.Open(path, FileMode.Open))
                {
                    return Image.FromStream(stream);
                }
            }

            return null;
        }

        public void Tap(Point point)
        {
            Run($"shell input tap {point.X} {point.Y}");
        }

        public void Swipe(Point from, Point to)
        {
            Run($"shell input swipe {from.X} {from.Y} {to.X} {to.Y}");
        }
        public void Tap(int x, int y)
        {
            Run($"shell input tap {x} {y}");
        }

        public void Swipe(int fromX, int fromY, int toX, int toY)
        {
            Run($"shell input swipe {fromX} {fromY} {toX} {toY}");
        }
    }
}