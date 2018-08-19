namespace AndroidHelper
{
    abstract class Parameter : IParameter
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public ParameterType Type { get; set; }
        public object Value { get; set; }
    }
    class Parameter<T> : Parameter, IParameter<T>
    {
        public override string ToString()
        {
            return Value.ToString();
        }
        public new T Value
        {
            get => base.Value is T ? (T)base.Value : default(T);
            set => base.Value = value;
        }
    }
}