using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AndroidHelper
{
    class Parser
    {
        public Script Parse(string script, string name, string desc)
        {
            var lines = script.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

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
                if (line.StartsWith("@name") || line.StartsWith("@desc"))
                {
                    continue;
                }
                if (line.StartsWith("loop:", StringComparison.OrdinalIgnoreCase))
                {
                    var lp = new Loop();
                    if (line.Length > 5)
                    {
                        var ps = line.Substring(5).Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                        lp.Name = ps[0];
                        if (ps.Length > 1)
                        {
                            lp.Total = GetValue(ps[1], ParameterType.Int, pars) as IValue<int>;
                        }
                        if (ps.Length > 2)
                        {
                            lp.ResetTimeout = GetValue(ps[2], ParameterType.Int, pars) as IValue<int>;
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
            bool isFast = line.StartsWith("*");
            if (isFast)
                line = line.Substring(1).Trim();
            if (line.StartsWith("tap", StringComparison.OrdinalIgnoreCase))
            {
                var pars = line.Substring(3).Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                var queue = new Queue<string>(pars);
                var point = GetValue(queue, ParameterType.Point, parameters);
                if (point is IValue<Point> p)
                {
                    return new TapCommand
                    {
                        Point = p,
                        Depth = depth,
                        IsFast = isFast
                    };
                }
            }
            if (line.StartsWith("swipe", StringComparison.OrdinalIgnoreCase))
            {
                var pars = line.Substring(5).Split(new[] { ' ' }, 4, StringSplitOptions.RemoveEmptyEntries);

                var queue = new Queue<string>(pars);
                var from = GetValue(queue, ParameterType.Point, parameters);
                var to = GetValue(queue, ParameterType.Point, parameters);
                if (from is IValue<Point> f && to is IValue<Point> t)
                {
                    return new SwipeCommand
                    {
                        From = f,
                        To = t,
                        Depth = depth,
                        IsFast = isFast
                    };
                }
            }
            if (line.StartsWith("#wait", StringComparison.OrdinalIgnoreCase))
            {
                var pars = line.Substring(5).Trim();
                if (pars.Length > 0)
                {
                    var timeout = GetValue(pars, ParameterType.Int, parameters);
                    if (timeout is IValue<int> i)
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

        IValue GetValue(Queue<string> queue, ParameterType type, List<IParameter> parameters)
        {
            var val = queue.Dequeue();
            if (type == ParameterType.Point)
            {
                if (!val.StartsWith("@") && queue.Any())
                {
                    var val2 = queue.Dequeue();
                    val = val + " " + val2;
                }
            }
            return GetValue(val, type, parameters);
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

        IValue GetValue(string input, ParameterType type, List<IParameter> parameters)
        {
            if (input.Length <= 0)
            {
                return null;
            }

            if (type == ParameterType.Int)
            {
                var p = parameters.FirstOrDefault(x => x.Name == input && x.Type == type);
                if (p != null)
                {
                    return p as IValue<int>;
                }
                if (int.TryParse(input, out var _i))
                {
                    return new RealValue<int>(_i);
                }
            }
            if (type == ParameterType.Point)
            {
                var p = parameters.FirstOrDefault(x => x.Name == input && x.Type == type);
                if (p != null)
                {
                    return p as IValue<Point>;
                }
                if (Point.TryParse(input, out var _p))
                {
                    return new RealValue<Point>(_p);
                }
            }
            return null;
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