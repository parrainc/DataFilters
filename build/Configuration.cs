using Nuke.Common.Tooling;

using System.ComponentModel;

namespace DataFilters.ContinuousIntegration
{
    [TypeConverter(typeof(TypeConverter<Configuration>))]
    public class Configuration : Enumeration
    {
        public static readonly Configuration Debug = new() { Value = nameof(Debug) };
        public static readonly Configuration Release = new() { Value = nameof(Release) };

        public static implicit operator string(Configuration configuration) => configuration.Value;
    }
}