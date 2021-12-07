using System.ComponentModel;

namespace GgCli.ValueParsers
{
    public enum Apps
    {
        [Description("Sales Managment App")] SalesManagement = 1,
        [Description("Sales Request App")] SalesRequest = 2,
        [Description("Sales Order App")] SalesOrder = 3,
        [Description("Sales Invoice App")] SalesInvoice = 4
    }
}