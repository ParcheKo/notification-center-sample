using System.Collections.Generic;
using System.Reflection;
using GgCli.Commands;
using GgCli.ValueParsers;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GgCli
{
    [HelpOption(Description = "Displays this guide information")]
    [VersionOptionFromMember(
        "-v|--version",
        MemberName = nameof(GetVersion))]
    [Command(
        FullName = CliFullName,
        Description = CliDescription,
        Name = "gg",
        UsePagerForHelpText = true)]
    [Subcommand(
        typeof(Deploy)
    )]
    public class Program
    {
        private const string CliFullName = "gg CLI";

        private const string CliDescription = "A simple command-line tool for automating most-used development processes like : \n" +
                                              "\t - Checking if services are up or healthy \n" +
                                              "\t - App deployments \n" +
                                              "\t - etc";

        private readonly ILogger<Program> _logger;

        [Argument(
            0,
            Name = "files",
            Description = "files to search in.")]
        public string[] Files { get; } = { "file1.txt", "file2.txt", "file3.txt" };

        [Option(
            Template = "-b|--bank-lines",
            Description = "number of blank lines",
            ValueName = "count")]
        public string[] BlankLines { get; set; }

        public static int Main(
            string[] args)
        {
            var services = new ServiceCollection()
                .AddSingleton(PhysicalConsole.Singleton)
                .BuildServiceProvider();

            var app = new CommandLineApplication<Program>();
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);
            app.ValueParsers.AddRange(
                new List<IValueParser>
                {
                    new EnvironmentsValueParser(),
                    new AppsValueParser()
                });
            return app.Execute(args);
        }

        private void OnExecute(
            CommandLineApplication app)
        {
            app.ShowHelp();
        }


        private static string GetVersion()
        {
            return typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
        }
    }
}