﻿using DataFilters.Grammar.Syntax;
using DataFilters.UnitTests.Helpers;
using FluentAssertions;

using FsCheck.Xunit;


using FsCheck;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DataFilters.UnitTests.Grammar.Syntax
{
    public class OneOfExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public OneOfExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void IsFilterExpression() => typeof(OneOfExpression).Should()
            .BeAssignableTo<FilterExpression>().And
            .Implement<IEquatable<OneOfExpression>>().And
            .HaveConstructor(new[] { typeof(FilterExpression[]) }).And
            .HaveProperty<IEnumerable<FilterExpression>>("Values");

        [Fact]
        public void Ctor_Throws_ArgumentNullException_When_Argument_Is_Null()
        {
            // Act
            Action action = () => new OneOfExpression(null);

            // Assert
            action.Should()
                .ThrowExactly<ArgumentNullException>($"The parameter {nameof(OneOfExpression)}'s constructor cannot be null");
        }

        public static IEnumerable<object[]> EqualsCases
        {
            get
            {
                yield return new object[]
                {
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    true,
                    "comparing two different instances with same data in same order"
                };

                yield return new object[]
                {
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    new OneOfExpression(new ConstantValueExpression("prop2"), new ConstantValueExpression("prop1")),
                    false,
                    "comparing two different instances with same data but the order does not matter"
                };

                yield return new object[]
                {
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop3")),
                    false,
                    "comparing two different instances with different data"
                };
            }
        }

        [Theory]
        [MemberData(nameof(EqualsCases))]
        public void ImplementsEqualsCorrectly(OneOfExpression first, object other, bool expected, string reason)
        {
            _outputHelper.WriteLine($"First instance : {first}");
            _outputHelper.WriteLine($"Second instance : {other}");

            // Act
            bool actual = first.Equals(other);
            int actualHashCode = first.GetHashCode();

            // Assert
            actual.Should()
                .Be(expected, reason);
            if (expected)
            {
                actualHashCode.Should()
                    .Be(other?.GetHashCode(), reason);
            }
            else
            {
                actualHashCode.Should()
                    .NotBe(other?.GetHashCode(), reason);
            }
        }

        public static IEnumerable<object[]> IsEquivalentToCases
        {
            get
            {
                yield return new object[]
                {
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    true,
                    "comparing two different instances with same data in same order"
                };

                yield return new object[]
                {
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    new OneOfExpression(new ConstantValueExpression("prop2"), new ConstantValueExpression("prop1")),
                    true,
                    "comparing two different instances with same data but the order does not matter"
                };

                yield return new object[]
                {
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop3")),
                    false,
                    "comparing two different instances with different data"
                };

                yield return new object[]
                {
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2"), new ConstantValueExpression("prop3")),
                    false,
                    "the other instance contains all data of the first instance and one item that is not in the current instance"
                };

                yield return new object[]
                {
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    new OneOfExpression(new ConstantValueExpression("prop1"), new ConstantValueExpression("prop2")),
                    true,
                    $"a {nameof(OneOfExpression)} instance that holds duplicates is equivalent a {nameof(OneOfExpression)} with no duplicate"
                };
            }
        }

        [Theory]
        [MemberData(nameof(IsEquivalentToCases))]
        public void Implements_IsEquivalentTo_Properly(OneOfExpression first, FilterExpression other, bool expected, string reason)
        {
            // Act
            bool actual = first.IsEquivalentTo(other);

            // Assert
            actual.Should()
                  .Be(expected, reason);
        }

        [Property(Arbitrary = new[] { typeof(ExpressionsGenerators) })]
        public Property Given_ConstantExpression_equals_left_and_left_equals_right_expression_IsEquivalentTo_should_be_true(ConstantValueExpression filterExpression, PositiveInt n)
            => Given_AllExpressions_are_equal_and_filterExpression_equal_to_one_expression_IsEquivalentTo_should_return_true(filterExpression, n.Item);

        [Property(Arbitrary = new[] { typeof(ExpressionsGenerators) })]
        public Property Given_DateExpression_equals_left_and_left_equals_right_expression_IsEquivalentTo_should_be_true(DateExpression filterExpression, PositiveInt n)
            => Given_AllExpressions_are_equal_and_filterExpression_equal_to_one_expression_IsEquivalentTo_should_return_true(filterExpression, n.Item);

        [Property(Arbitrary = new[] { typeof(ExpressionsGenerators) })]
        public Property Given_DateTimeExpression_equals_left_and_left_equals_right_expression_IsEquivalentTo_should_be_true(DateTimeExpression filterExpression, PositiveInt n)
            => Given_AllExpressions_are_equal_and_filterExpression_equal_to_one_expression_IsEquivalentTo_should_return_true(filterExpression, n.Item);

        [Property(Arbitrary = new[] { typeof(ExpressionsGenerators) })]
        public Property Given_AsteriskExpression_equals_left_and_left_equals_right_expression_IsEquivalentTo_should_be_true(AsteriskExpression filterExpression, PositiveInt n)
            => Given_AllExpressions_are_equal_and_filterExpression_equal_to_one_expression_IsEquivalentTo_should_return_true(filterExpression, n.Item);

        private static Property Given_AllExpressions_are_equal_and_filterExpression_equal_to_one_expression_IsEquivalentTo_should_return_true(FilterExpression filterExpression, int n)
        {
            // Arrange
            IEnumerable<FilterExpression> filterExpressions = Enumerable.Repeat(filterExpression, n);

            OneOfExpression oneOfExpression = new(filterExpressions.ToArray());

            // Act
            return oneOfExpression.IsEquivalentTo(filterExpression)
                                  .ToProperty();
        }
    }
}
