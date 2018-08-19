namespace AndroidHelper
{
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
        public bool IsFast { get; set; }
        public virtual void Reset()
        {

        }
        public virtual bool IsValid => true;
        protected abstract void RunCore(IScriptContext token);
    }
}