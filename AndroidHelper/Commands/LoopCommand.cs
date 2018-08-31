using System.Collections.Generic;
using System.Linq;

namespace AndroidHelper
{
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
                    token.Wait();
                    if (command.IsValid)
                    {
                        if (Global.Runner.IsTheGame() && !Global.Runner.IsLockedOrPowerOff())
                        {
                            command.Run(token);
                        }
                        else
                        {
                            token.Break("界面被切换或屏幕被关闭。");
                        }
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
                ? new string(' ', Depth * 4) + $"执行循环[{Name}]: {Count + 1} / {Total} 次"
                : new string(' ', Depth * 4) + $"执行循环[{Name}]: {Count + 1} / 无限 次";
        }
    }
}