using SignalBox.Core;
using Xunit;

namespace SignalBox.Test.Entities
{
    public class CustomerEntityTests
    {
        [Theory]
        [InlineData("xx xx xx")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("??^%")]
        [InlineData("symbol!")]
        [InlineData("symbol@")]
        [InlineData("symbol#")]
        [InlineData("symbol$")]
        [InlineData("symbol%")]
        [InlineData("symbol^")]
        [InlineData("symbol&")]
        [InlineData("symbol*")]
        [InlineData("symbol(")]
        [InlineData("symbol)")]
        [InlineData("symbol+")]
        public void CreateCustomer_BadCustomerId_Throws(string customerId)
        {
            Assert.Throws<CommonIdException>(() => new Customer(customerId));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("20")]
        [InlineData("symbol")]
        [InlineData("quitealongcustomerid")]
        [InlineData("underscore_and-hyphen")]
        [InlineData("allcombi-_|123")]
        public void CreateCustomer_GoodCustomerId(string customerId)
        {
            var customer = new Customer(customerId);
            Assert.Equal(customerId, customer.CustomerId);
        }
    }
}