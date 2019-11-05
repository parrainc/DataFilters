﻿using System;

namespace DataFilters.Grammar.Syntax
{
    /// <summary>
    /// An expression that negate wrapped inside
    /// </summary>
    public class NotExpression : FilterExpression, IEquatable<NotExpression>
    {
        /// <summary>
        /// Expression that the NOT logical is applied to
        /// </summary>
        public FilterExpression Expression { get; }

        /// <summary>
        /// Builds a new <see cref="NotExpression"/> that holds the specified <paramref name="innerExpression"/>.
        /// </summary>
        /// <param name="innerExpression"></param>
        /// <exception cref="ArgumentNullException"><paramref name="innerExpression"/> is <c>null</c>.</exception>
        public NotExpression(FilterExpression innerExpression) => Expression = innerExpression ?? throw new ArgumentNullException(nameof(innerExpression));

        public bool Equals(NotExpression other) => Expression.Equals(other?.Expression);

        public override bool Equals(object obj) => Equals(obj as NotExpression);

        public override int GetHashCode() => Expression.GetHashCode();

        public override string ToString() => $"{GetType().Name} : Expression ({Expression.GetType().Name}) -> {Expression}";
    }
}