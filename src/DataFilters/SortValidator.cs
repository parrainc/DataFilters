﻿using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataFilters
{
    /// <summary>
    /// Validates sort expression
    /// </summary>
    public class SortValidator : AbstractValidator<string>
    {
        private const string FieldPattern = Constants.ValidFieldNamePattern;
        public readonly static string Pattern = @$"^\s*(-|\+)?(({FieldPattern})\w*)+(\s*,\s*((-|\+)?(({FieldPattern})\w*)+)\s*)*$";
        private readonly Regex _sortRegex = new Regex(Pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
        private const char _separator = ',';

        public SortValidator() => RuleFor(x => x)
                .Matches(Pattern)
                .WithMessage(search =>
                {
                    string[] incorrectExpresions = search.Split(new[] { _separator })
                        .Where(x => !_sortRegex.IsMatch(x))
                        .Select(x => $@"""{x}""")
                        .ToArray();

                    return $"Sort expression{(incorrectExpresions.Length == 1 ? string.Empty : "s")} {string.Join(", ", incorrectExpresions)} " +
                    $@"do{(incorrectExpresions.Length == 1 ? "es" : string.Empty)} not match ""{Pattern}"".";
                });
    }
}
