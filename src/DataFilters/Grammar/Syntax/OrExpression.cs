﻿using System;

namespace DataFilters.Grammar.Syntax
{
    /// <summary>
    /// An expression that combine two <see cref="FilterExpression"/> expressions using the logical <c>OR</c> operator
    /// </summary>
    public sealed class OrExpression : FilterExpression, IEquatable<OrExpression>
    {
        /// <summary>
        /// Left member of the expression
        /// </summary>
        public FilterExpression Left { get; }

        /// <summary>
        /// Right member of the expression
        /// </summary>
        public FilterExpression Right { get; }

        /// <summary>
        /// Builds a new <see cref="OrExpression"/> that combines <paramref name="left"/> and <paramref name="right"/> using the logical
        /// <c>OR</c> operator
        /// </summary>
        /// <param name="left">Left member</param>
        /// <param name="right">Right member</param>
        /// <exception cref="ArgumentNullException">if either <paramref name="left"/> or <paramref name="right"/> is <c>null</c>.</exception>
        public OrExpression(FilterExpression left, FilterExpression right)
        {
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        ///<inheritdoc/>
        public bool Equals(OrExpression other) => Left.Equals(other?.Left) && Right.Equals(other?.Right);

        ///<inheritdoc/>
        public override bool Equals(object obj) => Equals(obj as OrExpression);

        ///<inheritdoc/>
#if NETSTANDARD1_3 || NETSTANDARD2_0
        public override int GetHashCode() => (Left, Right).GetHashCode();
#else
        public override int GetHashCode() => HashCode.Combine(Left, Right);
#endif

        ///<inheritdoc/>
        public override string ToString() => new
        {
            Left = new { Type = Left.GetType().Name, Left },
            Right = new { Type = Right.GetType().Name, Right },
        }.Jsonify();

        /// <inheritdoc/>
        public override bool IsEquivalentTo(FilterExpression other)
        {
            bool equivalent = false;

            if (other is OrExpression or)
            {
                equivalent = or.Right.Equals(Right) && or.Left.Equals(Left)
                    || (or.Left.Equals(Right) && or.Right.Equals(Left));
            }
            else
            {
                equivalent = Left.Equals(Right) && Left.Equals(other);
            }

            return equivalent;
        }
    }
}