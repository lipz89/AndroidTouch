using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AndroidHelper
{
    class Script : IScript
    {
        private event EventHandler stopped;
        private event EventHandler<NeedParameterArgs> needParameters;
        private readonly ScriptContext context;
        private readonly List<ICommand> commands;
        private readonly List<IParameter> parameters;
        private Thread thread;
        public Script(List<ICommand> commands, List<IParameter> parameters)
        {
            this.commands = commands.ToList();
            this.parameters = parameters.ToList();
            this.context = new ScriptContext();
            this.context.Status = Status.Inited;
        }
        public bool Start()
        {
            if (parameters.Any(x => x.Value == null))
            {
                if (!SetParameters())
                    return false;
            }
            context.Status = Status.Running;
            context.IsCancel = false;

            foreach (var command in commands)
            {
                command.Reset();
            }
            thread?.DisableComObjectEagerCleanup();
            thread?.Abort();
            thread = new Thread(() =>
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
                Thread.CurrentThread.DisableComObjectEagerCleanup();
                Thread.CurrentThread.Abort();
            });
            thread.IsBackground = true;
            thread.Start();
            return true;
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
        public bool HasParameters => this.parameters.Any();

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
        public event EventHandler<BrokenArgs> Broken
        {
            add => context.Broken += value;
            remove => context.Broken -= value;
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

        public void Dispose()
        {
            thread?.DisableComObjectEagerCleanup();
            thread?.Abort();
        }
    }
}