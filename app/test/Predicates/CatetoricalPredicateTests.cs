using Xunit;
using SignalBox.Core.Predicates;

namespace SignalBox.Test.Predicates
{
    public class CategoricalPredicateTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("something", "")]
        [InlineData("", "hello")]
        public void NoneOperator_ReturnsFalse(string value, string compareTo)
        {
            var pred = new CategoricalPredicate(CategoricalPredicateOperators.None, compareTo);
            Assert.False(pred.Evaluate(value));
        }

        [Theory]
        [InlineData("cats", CategoricalPredicateOperators.Equal, "cats", true)]
        [InlineData("dogs", CategoricalPredicateOperators.Equal, "cats", false)]
        [InlineData("dogs", CategoricalPredicateOperators.NotEqual, "dogs", false)]
        [InlineData("cats", CategoricalPredicateOperators.NotEqual, "dogs", true)]
        public void Operator_Evaluates_Correctly(string value, CategoricalPredicateOperators o, string compareTo, bool expected)
        {
            var pred = new CategoricalPredicate(o, compareTo);
            Assert.Equal(expected, pred.Evaluate(value));
        }
    }
}