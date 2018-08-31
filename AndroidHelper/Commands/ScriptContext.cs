using System;
using System.Threading;

namespace AndroidHelper
{
    class ScriptContext : IScriptContext
    {
        internal event EventHandler<CommondRunArgs> CommandRunned;
        internal event EventHandler<CommondRunArgs> CommandRunning;
        internal event EventHandler<BrokenArgs> Broken;

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

        public void Break(string reason)
        {
            Broken?.Invoke(this, new BrokenArgs() { Reason = reason });
        }
    }
}