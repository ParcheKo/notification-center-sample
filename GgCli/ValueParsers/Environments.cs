using System.ComponentModel;

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
}