namespace AndroidHelper
{
    interface ILoopCommand : ICommand
    {
        string Name { get; }
        int Count { get; }
        IValue<int> Total { get; }
        IValue<int> ResetTimeout { get; }
    }
}