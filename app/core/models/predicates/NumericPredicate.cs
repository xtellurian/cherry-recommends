using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Predicates
{
#nullable enable
    public class NumericPredicate : Predicate<double>
    {
        public NumericPredicate() { }

        public NumericPredicate(NumericPredicateOperators predicateOperator, double compareTo)
        {
            PredicateOperator = predicateOperator;
            CompareTo = compareTo;
        }

        // these need to be transmitted. i.e. they are DTOs
        public NumericPredicateOperators PredicateOperator { get; set; }
        public double CompareTo { get; set; }

        public bool Evaluate(double value)
        {
            var d = ToExpression().Compile();
            return d.Invoke(value);
        }

        public override Expression<Func<double, bool>> ToExpression()
        {
            return PredicateOperator switch
            {
                NumericPredicateOperators.Equal => (value) => value == CompareTo,
                NumericPredicateOperators.NotEqual => (value) => value != CompareTo,
                NumericPredicateOperators.GreaterThan => (value) => value > CompareTo,
                NumericPredicateOperators.LessThan => (value) => value < CompareTo,
                NumericPredicateOperators.GreaterThanOrEqualTo => (value) => value >= CompareTo,
                NumericPredicateOperators.LessThanOrEqualTo => (value) => value <= CompareTo,
                NumericPredicateOperators.None => (value) => false,
                _ => (value) => false,
            };
        }
    }
}