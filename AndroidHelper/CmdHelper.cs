using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Androids
{
    internal static class CmdHelper
    {
        public static void Start(string name, params string[] args)
        {
            name = name + ".exe";
            name = name.Replace(".exe.exe", ".exe");
            Process process = new Process();
            process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            process.StartInfo.FileName = name;
            if (args.Length > 0)
            {
                process.StartInfo.Arguments = string.Join(" ", args);
            }
            process.Start();
        }
        public static async Task<string> ExecuteDOS(string format, params string[] args)
        {
            var cmd = string.Format(format, args);
            return await ExecuteDOS(cmd);
        }

        private static string RemoveStart(string text, string start)
        {
            if (text.StartsWith(start))
            {
                text = text.Substring(start.Length);
            }
            return text.Trim();
        }
        private static string RemoveEnd(string text, string end)
        {
            if (text.EndsWith(end))
            {
                text = text.Substring(0, text.Length - end.Length);
            }
            return text.Trim();
        }
        public static async Task<string> ExecuteDOS(string cmd)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardInput.WriteLine("@echo off");
                process.StandardInput.WriteLine(cmd);
                process.StandardInput.WriteLine("exit");
                string text = await process.StandardOutput.ReadToEndAsync();
                text = text.Substring(text.IndexOf(cmd, StringComparison.Ordinal));
                return RemoveEnd(RemoveStart(text, cmd), "exit");
            }
        }

        public static async Task<string> ExecuteDOS(params string[] cmds)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardInput.WriteLine("@echo off");
                for (int i = 0; i < cmds.Length; i++)
                {
                    string value = cmds[i];
                    process.StandardInput.WriteLine(value);
                }

                process.StandardInput.WriteLine("exit");
                string text = await process.StandardOutput.ReadToEndAsync();
                text = text.Substring(text.IndexOf(cmds[0], StringComparison.Ordinal));
                return RemoveEnd(RemoveStart(text, cmds.Last()), "exit");
            }
        }
    }
}