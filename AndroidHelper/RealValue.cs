namespace AndroidHelper
{
    class RealValue<T> : IValue<T>
    {
        public RealValue(T value)
        {
            this.Value = value;
        }
        public T Value { get; set; }
        public override string ToString()
        {
            return Value.ToString();
        }
        object IValue.Value => Value;
    }
}