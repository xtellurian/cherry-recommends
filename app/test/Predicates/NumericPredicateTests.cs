using Xunit;
using SignalBox.Core.Predicates;

namespace SignalBox.Test
{
    public class NumericPredicateTests
    {
        [Theory]
        [InlineData(double.MaxValue, 0)]
        [InlineData(-1, double.MaxValue)]
        [InlineData(0, double.MinValue)]
        [InlineData(1, 0)]
        [InlineData(double.MinValue, 100)]
        public void NoneOperator_ReturnsFalse(double value, double compareTo)
        {
            var pred = new NumericPredicate(NumericPredicateOperators.None, compareTo);
            Assert.False(pred.Evaluate(value));
        }

        [Theory]
        [InlineData(5, NumericPredicateOperators.GreaterThan, 5, false)]
        [InlineData(5.1, NumericPredicateOperators.GreaterThan, 4.0, true)]
        [InlineData(5, NumericPredicateOperators.LessThan, 5, false)]
        [InlineData(4, NumericPredicateOperators.LessThan, 5, true)]
        [InlineData(5, NumericPredicateOperators.GreaterThanOrEqualTo, 5, true)]
        [InlineData(5, NumericPredicateOperators.LessThanOrEqualTo, 5, true)]
        public void Operator_Evaluates_Correctly(double value, NumericPredicateOperators o, double compareTo, bool expected)
        {
            var pred = new NumericPredicate(o, compareTo);
            Assert.Equal(expected, pred.Evaluate(value));
        }
    }
}