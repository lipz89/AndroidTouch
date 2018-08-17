using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace AndroidHelper
{
    class CommondRunArgs : EventArgs
    {
        internal ICommandInfo Commond { get; set; }
    }
    class NeedParameterArgs : EventArgs
    {
        internal List<IParameter> Parameters { get; set; }
        internal bool IsCancel { get; set; }
    }
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

                MobileSize = Runner.GetMobileSize();
            }
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
    interface IScript
    {
        void Start();
        void Stop();
        void Pause();
        void Continue();
        bool SetParameters();
        IScriptContext Context { get; }
        string Name { get; }
        string Desc { get; }

        event EventHandler Stopped;
        event EventHandler<CommondRunArgs> CommandRunning;
        event EventHandler<CommondRunArgs> CommandRunned;
        event EventHandler<NeedParameterArgs> NeedParameters;
    }

    interface ICommandInfo
    {
    }

    interface ICommand : ICommandInfo
    {
        void Run(IScriptContext token);
        bool IsValid { get; }
        void Reset();
        int Depth { get; }
    }

    interface IScriptContext
    {
        bool IsCancel { get; }
        Status Status { get; }
        void Runned(ICommandInfo command);
        void Running(ICommandInfo command);
        void Wait();
        void Set();
        void Reset();
    }
    class ScriptContext : IScriptContext
    {
        internal event EventHandler<CommondRunArgs> CommandRunned;
        internal event EventHandler<CommondRunArgs> CommandRunning;

        public ScriptContext()
        {
            resetEvent = new ManualResetEvent(true);
        }
        private ManualResetEvent resetEvent { get; set; }
        public bool IsCancel { get; set; }
        public Status Status { get; set; }
        public void Runned(ICommandInfo command)
        {
            CommandRunned?.Invoke(this, new CommondRunArgs { Commond = command });
        }
        public void Running(ICommandInfo command)
        {
            CommandRunning?.Invoke(this, new CommondRunArgs { Commond = command });
        }

        public void Wait()
        {
            resetEvent.WaitOne();
        }

        public void Set()
        {
            resetEvent.Set();
            Status = Status.Running;
        }

        public void Reset()
        {
            resetEvent.Reset();
            Status = Status.Paused;
        }
    }
    abstract class BaseCommond : ICommand
    {

        public int Depth { get; set; }
        public void Run(IScriptContext token)
        {
            if (token.IsCancel) return;
            token.Wait();
            if (token.IsCancel) return;
            token.Running(this);
            this.RunCore(token);
            token.Runned(this);
        }
        public virtual void Reset()
        {

        }
        public virtual bool IsValid => true;
        protected abstract void RunCore(IScriptContext token);
    }

    struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString()
        {
            return $"{X} {Y}";
        }
        public static bool operator ==(Point x, Point y)
        {
            return x.X == y.X && x.Y == y.Y;
        }
        public static bool operator !=(Point x, Point y)
        {
            return !(x == y);
        }
        //public bool Equals(Point other)
        //{
        //    return X == other.X && Y == other.Y;
        //}
        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(null, obj)) return false;
        //    return obj is Point && Equals((Point)obj);
        //}
        //public override int GetHashCode()
        //{
        //    return ToString().GetHashCode();
        //}
        public static bool TryParse(string input, out Point point)
        {
            point = new Point();
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var ps = input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ps.Length != 2)
            {
                return false;
            }

            if (int.TryParse(ps[0], out var x) && int.TryParse(ps[1], out var y))
            {
                point.X = x;
                point.Y = y;
                return true;
            }

            return false;
        }
    }

    class TapCommand : BaseCommond, ICommand
    {
        public IValue<Point> Point { get; set; }

        protected override void RunCore(IScriptContext token)
        {
            Global.Runner.Tap(Point.Value);
        }
        public override string ToString()
        {
            return new string(' ', Depth * 4) + $"点击：({Point})";
        }
    }

    class SwipeCommand : BaseCommond, ICommand
    {
        public IValue<Point> From { get; set; }
        public IValue<Point> To { get; set; }
        public override bool IsValid => From.Value != To.Value;
        protected override void RunCore(IScriptContext token)
        {
            Global.Runner.Swipe(From.Value, To.Value);
        }
        public override string ToString()
        {
            return new string(' ', Depth * 4) + $"滑动： 从({From})到({To})";
        }
    }

    class WaitCommond : BaseCommond, ICommand
    {
        public IValue<int> Timeout { get; set; }
        public override bool IsValid => Timeout.Value > 0;

        protected override void RunCore(IScriptContext token)
        {
            Thread.Sleep(Timeout.Value);
        }
        public override string ToString()
        {
            return new string(' ', Depth * 4) + $"等待 {Timeout} 毫秒";
        }
    }

    interface ILoopCommand : ICommand
    {
        string Name { get; }
        int Count { get; }
        IValue<int> Total { get; }
        IValue<int> ResetTimeout { get; }
    }

    class LoopCommand : BaseCommond, ILoopCommand
    {
        private readonly List<ICommand> commands;
        public IValue<int> Total { get; }
        public string Name { get; set; }
        public int Count { get; private set; }
        public IValue<int> ResetTimeout { get; }
        public override bool IsValid => this.commands.Any(x => x.IsValid);
        public LoopCommand(List<ICommand> commands, IValue<int> resetTimeout, IValue<int> total, int depth)
        {
            this.commands = commands.ToList();
            this.Total = total ?? new RealValue<int>(0);
            this.ResetTimeout = resetTimeout ?? new RealValue<int>(0);
            this.Depth = depth;
            this.commands.Add(new WaitCommond { Timeout = this.ResetTimeout, Depth = this.Depth + 1 });
        }
        protected override void RunCore(IScriptContext token)
        {
            while (!token.IsCancel && (Total.Value == 0 || Count < Total.Value))
            {
                Count++;
                token.Running(this);
                foreach (var command in this.commands)
                {
                    if (command.IsValid)
                    {
                        command.Run(token);
                    }
                }
            }
        }

        public override void Reset()
        {
            foreach (var command in commands)
            {
                command.Reset();
                this.Count = 0;
            }
        }

        public override string ToString()
        {
            return Total.Value > 0
                ? new string(' ', Depth * 4) + $"执行循环[{Name}]: {Count} / {Total} 次"
                : new string(' ', Depth * 4) + $"执行循环[{Name}]: {Count} / 无限 次";
        }
    }

    enum Status
    {
        Inited,
        Running,
        Cancelling,
        Cancelled,
        Paused,
        Finished
    }

    class Script : IScript
    {
        private event EventHandler stopped;
        private event EventHandler<NeedParameterArgs> needParameters;
        private readonly ScriptContext context;
        private readonly List<ICommand> commands;
        private readonly List<IParameter> parameters;
        public Script(List<ICommand> commands, List<IParameter> parameters)
        {
            this.commands = commands.ToList();
            this.parameters = parameters.ToList();
            this.context = new ScriptContext();
            this.context.Status = Status.Inited;
        }


        public void Start()
        {
            if (parameters.Any(x => x.Value == null))
            {
                if (!SetParameters())
                    return;
            }
            context.Status = Status.Running;
            context.IsCancel = false;

            foreach (var command in commands)
            {
                command.Reset();
            }

            var thread = new Thread(() =>
            {
                foreach (var command in commands)
                {
                    if (command.IsValid)
                    {
                        command.Run(context);
                    }
                }

                context.Status = context.IsCancel ? Status.Cancelled : Status.Finished;
                stopped?.Invoke(this, EventArgs.Empty);
            });
            thread.Start();
        }
        public void Stop()
        {
            context.IsCancel = true;
            if (context.Status == Status.Paused)
            {
                context.Set();
            }
            context.Status = Status.Cancelling;
        }
        public void Pause()
        {
            context.Reset();
        }
        public void Continue()
        {
            context.Set();
        }

        public bool SetParameters()
        {
            if (parameters.Any())
            {
                var args = new NeedParameterArgs { Parameters = parameters };
                needParameters?.Invoke(null, args);
                if (args.IsCancel)
                {
                    return false;
                }
            }
            return true;
        }

        public IScriptContext Context => context;
        public string Name { get; set; }
        public string Desc { get; set; }

        public event EventHandler<CommondRunArgs> CommandRunning
        {
            add => context.CommandRunning += value;
            remove => context.CommandRunning -= value;
        }

        public event EventHandler<CommondRunArgs> CommandRunned
        {
            add => context.CommandRunned += value;
            remove => context.CommandRunned -= value;
        }

        public event EventHandler Stopped
        {
            add => stopped += value;
            remove => stopped -= value;
        }
        public event EventHandler<NeedParameterArgs> NeedParameters
        {
            add => needParameters += value;
            remove => needParameters -= value;
        }
    }

    enum ParameterType
    {
        String,
        Int,
        Point,
    }

    interface IValue
    {
        object Value { get; }
    }
    interface IValue<T> : IValue
    {
        new T Value { get; }
    }

    class RealValue<T> : IValue<T>
    {
        public RealValue(T value)
        {
            this.Value = value;
        }
        public T Value { get; set; }
        public override string ToString()
        {
            return Value.ToString();
        }

        object IValue.Value => Value;
    }

    interface IParameter : IValue
    {
        string Name { get; }
        string Display { get; }
        ParameterType Type { get; }
        new object Value { get; set; }
    }
    interface IParameter<T> : IParameter, IValue<T>
    {
        new T Value { get; set; }
    }

    abstract class Parameter : IParameter
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public ParameterType Type { get; set; }
        //object IValue.Value => Value;
        public object Value { get; set; }
    }

    class Parameter<T> : Parameter, IParameter<T>
    {
        public override string ToString()
        {
            return Value.ToString();
        }
        public new T Value
        {
            get => base.Value is T ? (T)base.Value : default(T);
            set => base.Value = value;
        }

        //T IValue<T>.Value { get => base.Value is T ? (T)base.Value : default(T); }
    }

    class Parser
    {
        private static Regex parName = new Regex("@[a-z]*", RegexOptions.IgnoreCase);
        public Script Parse(string script)
        {
            var lines = script.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

            string name = "[未命名]";
            var nameLine = lines.FirstOrDefault(x => x.StartsWith("@name", StringComparison.OrdinalIgnoreCase));
            if (nameLine != null)
            {
                name = nameLine.Replace("@name", "").Trim();
            }

            string desc = "";
            var descLine = lines.FirstOrDefault(x => x.StartsWith("@desc", StringComparison.OrdinalIgnoreCase));
            if (descLine != null)
            {
                desc = descLine.Replace("@desc", "").Trim();
            }
            var loopCommands = new Stack<Loop>();
            loopCommands.Push(new Loop());
            var pars = new List<IParameter>();
            foreach (var line in lines.Where(x => x.StartsWith("@par", StringComparison.OrdinalIgnoreCase)))
            {
                var par = ParseParameter(line);
                if (par != null) pars.Add(par);
            }

            foreach (var line in lines.Where(x => !x.StartsWith("@")))
            {
                //var txtLine = parName.Replace(line, match => pars.First(x => x.Name == match.Value).Value.ToString());

                if (line.StartsWith("loop:", StringComparison.OrdinalIgnoreCase))
                {
                    var lp = new Loop();
                    if (line.Length > 5)
                    {
                        var ps = line.Substring(5).Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                        lp.Name = ps[0];
                        if (ps.Length > 1)
                        {
                            var p = pars.FirstOrDefault(x => x.Name == ps[1] && x.Type == ParameterType.Int);
                            if (p != null)
                            {
                                lp.Total = p as IValue<int>;
                            }
                            else if (int.TryParse(ps[1], out int _loopTotal))
                            {
                                lp.Total = new RealValue<int>(_loopTotal);
                            }
                        }
                        if (ps.Length > 2)
                        {
                            var p = pars.FirstOrDefault(x => x.Name == ps[2] && x.Type == ParameterType.Int);
                            if (p != null)
                            {
                                lp.ResetTimeout = p as IValue<int>;
                            }
                            else if (int.TryParse(ps[2], out int _loopResetTimeout))
                            {
                                lp.ResetTimeout = new RealValue<int>(_loopResetTimeout);
                            }
                        }
                    }
                    loopCommands.Push(lp);
                    continue;
                }
                if (line.StartsWith("loopend", StringComparison.OrdinalIgnoreCase))
                {
                    var cmds = loopCommands.Pop();
                    var loop = new LoopCommand(cmds.Commands, cmds.ResetTimeout, cmds.Total, loopCommands.Count - 1)
                    {
                        Name = cmds.Name
                    };
                    loopCommands.Peek().Commands.Add(loop);
                    continue;
                }

                var cmd = ParseCommand(line, pars, loopCommands.Count - 1);
                if (cmd != null)
                {
                    loopCommands.Peek().Commands.Add(cmd);
                }
            }

            return new Script(loopCommands.Pop().Commands, pars) { Name = name, Desc = desc };
        }

        ICommand ParseCommand(string line, List<IParameter> parameters, int depth = 0)
        {
            if (line.StartsWith("tap", StringComparison.OrdinalIgnoreCase))
            {
                var pars = line.Substring(3).Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (pars.Length == 2 &&
                    int.TryParse(pars[0], out int x) &&
                    int.TryParse(pars[1], out int y))
                {
                    return new TapCommand
                    {
                        Point = new RealValue<Point>(new Point { X = x, Y = y }),
                        Depth = depth
                    };
                }
                if (pars.Length == 1 && pars[0].StartsWith("@"))
                {
                    var arg = parameters.FirstOrDefault(p => p.Name == pars[0]);
                    if (arg is IValue<Point> point)
                    {
                        return new TapCommand
                        {
                            Point = point,
                            Depth = depth
                        };
                    }
                }
            }
            if (line.StartsWith("swipe", StringComparison.OrdinalIgnoreCase))
            {
                var pars = line.Substring(5).Split(new[] { ' ' }, 4, StringSplitOptions.RemoveEmptyEntries);
                if (pars.Length == 4)
                {
                    if (int.TryParse(pars[0], out int fromx) &&
                        int.TryParse(pars[1], out int fromy) &&
                        int.TryParse(pars[2], out int tox) &&
                        int.TryParse(pars[3], out int toy))
                    {
                        return new SwipeCommand
                        {
                            From = new RealValue<Point>(new Point { X = fromx, Y = fromy }),
                            To = new RealValue<Point>(new Point { X = tox, Y = toy }),
                            Depth = depth
                        };
                    }
                }
                if (pars.Length == 3)
                {
                    if (int.TryParse(pars[0], out int fromx) &&
                        int.TryParse(pars[1], out int fromy) &&
                        pars[2].StartsWith("@"))
                    {
                        var arg2 = parameters.FirstOrDefault(p => p.Name == pars[0]);
                        if (arg2 is IValue<Point> point2)
                        {
                            return new SwipeCommand
                            {
                                From = new RealValue<Point>(new Point { X = fromx, Y = fromy }),
                                To = point2,
                                Depth = depth
                            };
                        }
                    }
                    if (int.TryParse(pars[1], out int tox) &&
                        int.TryParse(pars[2], out int toy) &&
                        pars[0].StartsWith("@"))
                    {
                        var arg1 = parameters.FirstOrDefault(p => p.Name == pars[0]);
                        if (arg1 is IValue<Point> point1)
                        {
                            return new SwipeCommand
                            {
                                From = point1,
                                To = new RealValue<Point>(new Point { X = tox, Y = toy }),
                                Depth = depth
                            };
                        }
                    }
                }
                if (pars.Length == 2 && pars[0].StartsWith("@") && pars[1].StartsWith("@"))
                {
                    var arg1 = parameters.FirstOrDefault(p => p.Name == pars[0]);
                    var arg2 = parameters.FirstOrDefault(p => p.Name == pars[0]);
                    if (arg1 is IValue<Point> point1 && arg2 is IValue<Point> point2)
                    {
                        return new SwipeCommand
                        {
                            From = point1,
                            To = point2,
                            Depth = depth
                        };
                    }
                }
            }
            if (line.StartsWith("#wait", StringComparison.OrdinalIgnoreCase))
            {
                var pars = line.Substring(5).Trim();
                if (pars.Length > 0)
                {
                    if (int.TryParse(pars, out int timeout))
                    {
                        return new WaitCommond
                        {
                            Timeout = new RealValue<int>(timeout),
                            Depth = depth
                        };
                    }
                    var arg = parameters.FirstOrDefault(x => x.Name == pars);
                    if (arg is IValue<int> i)
                    {
                        return new WaitCommond
                        {
                            Timeout = i,
                            Depth = depth
                        };
                    }
                }
            }
            throw new Exception("命令解析错误：" + line);
        }


        IParameter ParseParameter(string line)
        {
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("@par"))
            {
                return null;
            }

            var args = line.Substring(4).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var nv = args[0].Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (args.Length > 2)
            {
                if (Enum.TryParse<ParameterType>(args[2], true, out var type))
                {
                    Parameter parameter;
                    if (type == ParameterType.Point)
                    {
                        Parameter<Point> par = new Parameter<Point>();
                        if (nv.Length > 1 && Point.TryParse(nv[1], out var point))
                        {
                            par.Value = point;
                        }
                        parameter = par;
                    }
                    else if (type == ParameterType.Int)
                    {
                        Parameter<int> par = new Parameter<int>();
                        if (nv.Length > 1 && int.TryParse(nv[1], out var i))
                        {
                            par.Value = i;
                        }
                        parameter = par;
                    }
                    else
                    {
                        Parameter<string> par = new Parameter<string>();
                        if (nv.Length > 1)
                        {
                            par.Value = nv[1];
                        }
                        parameter = par;
                    }
                    parameter.Name = "@" + nv[0];

                    parameter.Display = args.Length > 1 ? args[1] : nv[0];
                    parameter.Type = type;
                    return parameter;
                }
            }

            throw new Exception("参数解析错误：" + line);
        }

        class Loop
        {
            public string Name { get; set; }
            public IValue<int> Total { get; set; }
            public IValue<int> ResetTimeout { get; set; }
            public List<ICommand> Commands { get; }

            public Loop()
            {
                Commands = new List<ICommand>();
            }
        }
    }
}