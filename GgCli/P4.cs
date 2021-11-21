using System;
using System.Reflection;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace GgCli
{
    [HelpOption(Description = "Displays this guide information")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Command(
        FullName = CliFullName,
        Description = CliDescription,
        Name = "gg",
        UsePagerForHelpText = true)]
    // [Subcommand(
    //     )]
    public class Cli
    {
        private const string CliFullName = "gg CLI";

        private const string CliDescription = "A simple command-line tool for automating most-used development processes like : \n" +
                                              "\t - Checking if services are up or healthy \n" +
                                              "\t - App deployments \n" +
                                              "\t - etc";

        [Argument(0, Name = "files", Description = "files to search in")]
        public string[] Files { get; } = { "file1.txt", "file2.txt", "file3.txt" };

        [Option(Template = "-b|--bank-lines", Description = "number of blank lines", ValueName = "count")]
        public string[] BlankLines { get; set; }

        private static Task<int> Main(string[] args)
        {
            return CommandLineApplication.ExecuteAsync<Cli>(args);
        }

        private async Task OnExecuteAsync()
        {
            if (Files is not null) Console.WriteLine($"You provided {string.Join(",", Files)} for Files");
        }

        private static string GetVersion()
        {
            return typeof(Git).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }
    }
}