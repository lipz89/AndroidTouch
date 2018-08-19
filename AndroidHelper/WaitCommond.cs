using System.Threading;

namespace AndroidHelper
{
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
}