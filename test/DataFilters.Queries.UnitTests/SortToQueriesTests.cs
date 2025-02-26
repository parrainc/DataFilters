﻿using FluentAssertions;
using Queries.Core.Parts.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;
using static Queries.Core.Parts.Sorting.OrderDirection;

namespace DataFilters.Queries.UnitTests
{
    public class SortToQueriesTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public SortToQueriesTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

        public class Person
        {
            public string Firstname { get; set; }

            public string Lastname { get; set; }
        }

        public static IEnumerable<object[]> SortToSortCases
        {
            get
            {
                yield return new object[]
                {
                    new Sort<Person>("Name"),
                    (Expression<Func<IEnumerable<IOrder>, bool>>) (sorts => sorts.Once()
                        && sorts.First().Equals(new OrderExpression("Name".Field(), Ascending)))
                };

                {
                    MultiSort<Person> multiSort = new
                    (
                        new Sort<Person>(nameof(Person.Firstname)),
                        new Sort<Person>(nameof(Person.Lastname))
                    );
                    yield return new object[]
                    {
                        multiSort,
                        (Expression<Func<IEnumerable<IOrder>, bool>>) (sorts => sorts.Exactly(2)
                            && sorts.First().Equals(new OrderExpression(nameof(Person.Firstname), Ascending))
                            && sorts.Last().Equals(new OrderExpression(nameof(Person.Lastname), Ascending)))
                    };
                }
            }
        }

        [Theory]
        [MemberData(nameof(SortToSortCases))]
        public void FilterToSort(ISort<Person> sort, Expression<Func<IEnumerable<IOrder>, bool>> expected)
        {
            _outputHelper.WriteLine($"Sort : {sort.Jsonify()}");

            // Act
            IEnumerable<IOrder> actual = sort.ToOrder();
            _outputHelper.WriteLine($"actual result : {actual.Jsonify()}");

            // Assert
            actual.Should()
                .Match(expected);
        }
    }
}
