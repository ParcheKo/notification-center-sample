using System;
using System.ComponentModel;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils.Abstractions;

namespace GgCli.ValueParsers
{
    public enum Environments
    {
        [Description("Development environment")]
        Development = 1,

        [Description("Staging/Test/QA environment")]
        Staging = 2,

        [Description("Production/Operational environment")]
        Production = 3
    }

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
            // Console.WriteLine("Converting...");
            // if (argName != "env" /*nameof(Deploy.Environment*/)
            // {
            //     return null;
            // }

            // Console.WriteLine("Converted Successfully!");
            return value.ToLower() switch
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