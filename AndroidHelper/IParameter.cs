namespace AndroidHelper
{
    interface IParameter : IValue
    {
        string Name { get; }
        string Display { get; }
        ParameterType Type { get; }
        new object Value { get; set; }
    }
    interface IParameter<T> : IParameter, IValue<T>
    {
        new T Value { get; set; }
    }
}