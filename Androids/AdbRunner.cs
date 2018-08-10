using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Androids
{
    internal class AdbRunner
    {
        private readonly string adbPath;
        private const string IMG_MOBILE_PATH = "/sdcard/_1.png";

        public AdbRunner(string adbPath)
        {
            this.adbPath = adbPath;
        }

        private string RunAdb(string cmd)
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

                var text = process.StandardOutput.ReadToEndAsync().Result;
                return text.Trim();
            }
        }

        private string Run(string command)
        {
            return RunAdb(command);
        }

        public string GetMobileInfo()
        {
            return Run("shell getprop ro.product.model");
        }

        public bool Connect(string ip, uint port)
        {
            Run("tcpip " + port);
            Run("connect " + ip);
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
        public string GetShotScreent(string path)
        {
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
            return path;
        }

        public void Tap(int x, int y)
        {
            Run($"shell input tap {x} {y}");
        }
    }
}