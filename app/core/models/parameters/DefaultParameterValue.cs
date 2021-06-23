namespace SignalBox.Core
{
    public class DefaultParameterValue
    {
        public DefaultParameterValue()
        { }
        public DefaultParameterValue(ParameterTypes parameterType, object value)
        {
            ParameterType = parameterType;
            Value = value;
        }

        public ParameterTypes ParameterType { get; set; }
        public object Value { get; set; }

        public T GetValue<T>()
        {
            var tType = typeof(T);

            if (tType == typeof(string) && ParameterType != ParameterTypes.Categorical)
            {
                throw new StorageException("Parameter Type Categorical does not match requested default type.");
            }
            else if (tType == typeof(double) && ParameterType != ParameterTypes.Numerical)
            {
                throw new StorageException("Parameter Type Numerical does not match requested default type.");
            }

            return (T)Value;
        }
    }
}