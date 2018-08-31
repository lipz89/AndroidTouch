namespace AndroidHelper
{
    interface IScriptContext
    {
        bool IsCancel { get; }
        Status Status { get; }
        void Runned(ICommandInfo command);
        void Running(ICommandInfo command);
        void Wait();
        void Set();
        void Reset();
        void Break(string reason);
    }
}