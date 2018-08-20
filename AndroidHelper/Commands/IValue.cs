namespace AndroidHelper
{
    interface IValue
    {
        object Value { get; }
    }
    interface IValue<T> : IValue
    {
        new T Value { get; }
    }
}