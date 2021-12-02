using System;
using System.ComponentModel;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils.Abstractions;

namespace GgCli.ValueParsers
{
    public enum Apps
    {
        [Description("Sales Managment App")] SalesManagement = 1,
        [Description("Sales Request App")] SalesRequest = 2,
        [Description("Sales Order App")] SalesOrder = 3,
        [Description("Sales Invoice App")] SalesInvoice = 4
    }

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
            // Console.WriteLine("Converting...");
            // if (argName != "env" /*nameof(Deploy.Environment*/)
            // {
            //     return null;
            // }

            // Console.WriteLine("Converted Successfully!");
            return value.ToLower() switch
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