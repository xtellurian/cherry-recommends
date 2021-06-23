
namespace SignalBox.Core.Recommenders
{
    public class DefaultArgumentValue
    {
        public DefaultArgumentValue()
        { }
        public DefaultArgumentValue(ArgumentTypes argumentType, object value)
        {
            ArgumentType = argumentType;
            Value = value;
        }

        public ArgumentTypes ArgumentType { get; set; }
        public object Value { get; set; }

        public T GetValue<T>()
        {
            var tType = typeof(T);

            if (tType == typeof(string) && ArgumentType != ArgumentTypes.Categorical)
            {
                throw new StorageException("Argument Type Categorical does not match requested default type.");
            }
            else if (tType == typeof(double) && ArgumentType != ArgumentTypes.Numerical)
            {
                throw new StorageException("Argumnet Type Numerical does not match requested default type.");
            }

            return (T)Value;
        }
    }
}