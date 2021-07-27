namespace SignalBox.Core
{
    public class DefaultParameterValue
    {
        public DefaultParameterValue()
        { }
        public DefaultParameterValue(ParameterTypes parameterType, object value)
        {
            ParameterType = parameterType;
            if (!string.IsNullOrEmpty(value?.ToString()))
            {
                if (parameterType == ParameterTypes.Numerical)
                {
                    if (int.TryParse(value.ToString(), out var iVal))
                    {
                        this.Value = iVal;
                    }
                    else if (double.TryParse(value.ToString(), out var dVal))
                    {
                        this.Value = dVal;
                    }
                    else
                    {
                        throw new BadRequestException($"The default value {value} is not Numerical");
                    }
                }
                else if (parameterType == ParameterTypes.Categorical)
                {
                    this.Value = value.ToString();
                }
                else
                {
                    throw new BadRequestException($"Unknown parameter type {parameterType}");
                }
            }
            else if (value?.ToString() == "")
            {
                this.Value = value.ToString();
            }
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