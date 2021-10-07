using SignalBox.Core;

namespace SignalBox.Web
{
    public class AllowApiKeyAttribute : System.Attribute
    {

        public AllowApiKeyAttribute()
        {
            KeyType = ApiKeyTypes.Server | ApiKeyTypes.Web;
        }

        public AllowApiKeyAttribute(ApiKeyTypes keyType)
        {
            KeyType = keyType;
        }

        public ApiKeyTypes KeyType { get; }
    }
}