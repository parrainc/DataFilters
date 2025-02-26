﻿using DataFilters.Grammar.Syntax;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Xunit.Categories;

namespace DataFilters.UnitTests.Grammar.Syntax
{
    [UnitTest]
    [Feature(nameof(AsteriskExpression))]
    public class AsteriskExpressionTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public AsteriskExpressionTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void IsFilterExpression() => typeof(AsteriskExpression).Should()
            .BeAssignableTo<FilterExpression>().And
            .Implement<IEquatable<AsteriskExpression>>().And
            .HaveDefaultConstructor();

        public static IEnumerable<object[]> EqualsCases
        {
            get
            {
                yield return new object[] { new AsteriskExpression(), null, false, "Comparing to null" };
                yield return new object[] { new AsteriskExpression(), new AsteriskExpression(), true, "Comparing to null" };
            }
        }

        [Theory]
        [MemberData(nameof(EqualsCases))]
        public void TestEquals(AsteriskExpression first, object other, bool expected, string reason)
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
    }
}
