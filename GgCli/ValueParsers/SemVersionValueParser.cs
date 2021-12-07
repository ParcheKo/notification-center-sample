using System;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils.Abstractions;

namespace GgCli.ValueParsers
{
    public class SemVersionValueParser : IValueParser<SemVersion?>
    {
        object? IValueParser.Parse(
            string? argName,
            string? value,
            CultureInfo culture)
        {
            return Parse(
                argName,
                value,
                culture);
        }

        public SemVersion? Parse(
            string? argName,
            string? value,
            CultureInfo culture)
        {
            return SemVersion.From(value!);
        }

        public Type TargetType { get; } = typeof(SemVersion);
    }
}