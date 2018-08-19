namespace AndroidHelper
{
    interface ICommand : ICommandInfo
    {
        void Run(IScriptContext token);
        bool IsValid { get; }
        bool IsFast { get; }
        void Reset();
        int Depth { get; }
    }
}