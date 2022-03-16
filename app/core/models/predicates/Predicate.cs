using System;
using System.Linq.Expressions;

namespace SignalBox.Core.Predicates
{

    public abstract class Predicate<TInput>
    {
        public abstract Expression<Func<TEntity, bool>> ToExpression<TEntity>(Expression<Func<TEntity, double?>> selectValue);
        public abstract Expression<Func<TInput, bool>> ToExpression();
    }
}