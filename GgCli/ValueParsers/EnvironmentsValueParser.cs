using System;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils.Abstractions;

namespace GgCli.ValueParsers
{
    public class EnvironmentsValueParser : IValueParser<Environments?>
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

        public Environments? Parse(
            string? argName,
            string? value,
            CultureInfo culture)
        {
            return value!.ToLower() switch
            {
                "dev" or nameof(Environments.Development) => Environments.Development,
                "stag" or "qa" or "test" or nameof(Environments.Staging) => Environments.Staging,
                "prod" or nameof(Environments.Production) => Environments.Production,
                _ => null
            };
        }

        public Type TargetType { get; } = typeof(Environments?);
    }
}