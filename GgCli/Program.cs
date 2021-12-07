using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using GgCli.Commands;
using GgCli.ValueParsers;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GgCli
{
    [VersionOptionFromMember(
        "-v|--version",
        MemberName = nameof(GetVersion))]
    [Command(
        FullName = FullName,
        Description = Description,
        Name = Name,
        UsePagerForHelpText = true)]
    public class Program
    {
        private const string Name = "gg";
        private const string FullName = "gg Command-Line Interface (CLI)";

        private const string Description = "A simple command-line tool for automating most-used development processes like : \n" +
                                           "\t - Checking if services are up or healthy \n" +
                                           "\t - App deployments \n" +
                                           "\t - etc";

        private readonly ILogger<Program> _logger;

        public static async Task<int> Main(
            string[] args)
        {
            Console.ResetColor();
            var services = new ServiceCollection()
                .AddSingleton(PhysicalConsole.Singleton)
                .BuildServiceProvider();

            var app = new CommandLineApplication<Program>();
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);
            var valueParsers = new List<IValueParser>
            {
                new EnvironmentsValueParser(),
                new AppsValueParser(),
                new SemVersionValueParser()
            };
            app.ValueParsers.AddRange(
                valueParsers);
            app.MakeSuggestionsInErrorMessage = true;

            var appArgument = DeployCommandDefinition.BuildAppArgument();
            appArgument.Validators.Add(new DeployCommandValidator());

            var environmentOption = DeployCommandDefinition.BuildEnvironmentOption();
            environmentOption.Validators.Add(new DeployCommandValidator());

            var versionOption = DeployCommandDefinition.BuildVersionOption();
            versionOption.Validators.Add(new DeployCommandValidator());

            app.Command(
                DeployCommandDefinition.Name,
                cmd =>
                {
                    cmd.FullName = GetFullName(
                        Name,
                        DeployCommandDefinition.Name);
                    cmd.AddName(DeployCommandDefinition.Alias);
                    cmd.AddArgument(appArgument);
                    cmd.AddOption(environmentOption);
                    cmd.AddOption(versionOption);
                    cmd.OnExecuteAsync(
                        cancellationToken => DeployCommandDefinition.OnExecuteAsync(
                            cancellationToken,
                            appArgument.ParsedValue,
                            environmentOption.ParsedValue,
                            versionOption.ParsedValue
                        ));
                });
            try
            {
                // return await app.ExecuteAsync(new []
                // {
                //     "d", "mng", "-e", "dev2", "-v", "1.2.3"
                // });
                return await app.ExecuteAsync(args);
            }
            catch (Exception e)
            {
                ColorfulConsole.WriteLineInColor(
                    e.Message,
                    ConsoleColor.Red);
                Console.ResetColor();
                return 1;
                // throw;
            }
        }

        private static string GetFullName(
            string parentCommandName,
            string commandName)
        {
            return $"{parentCommandName} {commandName}";
        }

        private void OnExecute(
            CommandLineApplication app)
        {
            ColorfulConsole.WriteLineInColor(
                "No command provided!",
                ConsoleColor.Yellow);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            app.ShowHelp();
            Console.ResetColor();
        }


        private static string GetVersion()
        {
            return typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
        }

        private static class ColorfulConsole
        {
            public static void WriteLineInColor(
                string text,
                ConsoleColor color)
            {
                var previousColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ForegroundColor = previousColor;
            }
        }
    }
}