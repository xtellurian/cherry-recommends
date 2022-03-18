using System;
using System.Linq.Expressions;

namespace SignalBox.Core.Predicates
{
#nullable enable
    public class CategoricalPredicate : Predicate<string>
    {
        public CategoricalPredicate() { }

        public CategoricalPredicate(CategoricalPredicateOperators predicateOperator, string compareTo)
        {
            PredicateOperator = predicateOperator;
            CompareTo = compareTo;
        }

        // these need to be transmitted. i.e. they are DTOs
        public CategoricalPredicateOperators PredicateOperator { get; set; }
        public string? CompareTo { get; set; }

        public bool Evaluate(string value)
        {
            var d = ToExpression().Compile();
            return d.Invoke(value);
        }

        public override Expression<Func<string, bool>> ToExpression()
        {
            return PredicateOperator switch
            {
                CategoricalPredicateOperators.Equal => (value) => value == CompareTo,
                CategoricalPredicateOperators.NotEqual => (value) => value != CompareTo,
                CategoricalPredicateOperators.None => (value) => false,
                _ => (value) => false,
            };
        }
    }
}