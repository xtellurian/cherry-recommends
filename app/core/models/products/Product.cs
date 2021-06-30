namespace SignalBox.Core
{
    public class Product : CommonEntity
    {
        protected Product()
        { }

        public Product(string commonId, string name, double listPrice, double? directCost = null) : base(commonId, name)
        {
            ListPrice = listPrice;
            DirectCost = directCost;
        }

        public double ListPrice { get; set; }
        public double? DirectCost { get; set; }
#nullable enable
        public string? Description { get; set; }
    }
}