using System;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils.Abstractions;

namespace GgCli.ValueParsers
{
    public class AppsValueParser : IValueParser<Apps?>
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

        public Apps? Parse(
            string? argName,
            string? value,
            CultureInfo culture)
        {
            return value!.ToLower() switch
            {
                "slmng" or "mng" or nameof(Apps.SalesManagement) => Apps.SalesManagement,
                "slreq" or "req" or nameof(Apps.SalesRequest) => Apps.SalesRequest,
                "slord" or "ord" or nameof(Apps.SalesOrder) => Apps.SalesOrder,
                "slinv" or "inv" or nameof(Apps.SalesInvoice) => Apps.SalesInvoice,
                _ => null
            };
        }

        public Type TargetType { get; } = typeof(Apps?);
    }
}