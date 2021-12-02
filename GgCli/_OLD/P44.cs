// using System;
// using System.ComponentModel.DataAnnotations;
// using System.Globalization;
// using System.Reflection;
// using System.Threading.Tasks;
// using McMaster.Extensions.CommandLineUtils;
// using McMaster.Extensions.CommandLineUtils.Abstractions;
//
//
//     [HelpOption(Description = "Displays this guide information")]
//     [VersionOptionFromMember("-v|--version", MemberName = nameof(GetVersion))]
//     [Command(
//         FullName = CliFullName,
//         Description = CliDescription,
//         Name = "gg",
//         UsePagerForHelpText = true)]
//     [Subcommand(
//         typeof(Deploy)
//     )]
//     public class Cli
//     {
//         private static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Cli>(args);
//
//         private const string CliFullName = "gg CLI";
//
//         private const string CliDescription = "A simple command-line tool for automating most-used development processes like : \n" +
//                                               "\t - Checking if services are up or healthy \n" +
//                                               "\t - App deployments \n" +
//                                               "\t - etc";
//
//         [Argument(0, Name = "files", Description = "files to search in.")]
//         public string[] Files { get; } = { "file1.txt", "file2.txt", "file3.txt" };
//
//         [Option(Template = "-b|--bank-lines", Description = "number of blank lines", ValueName = "count")]
//         public string[] BlankLines { get; set; }
//
//         private void OnExecute(CommandLineApplication app)
//         {
//             app.ShowHelp();
//         }
//
//
//         private static string GetVersion()
//         {
//             return typeof(Git).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
//         }
//     }
//
//     [Command(Description = "Does app deployments", Name = "d", FullName = "deploy")]
//     public class Deploy
//     {
//         [Argument(0, Name = "app", Description = "App to deploy"), Required]
//         private Apps App { get; set; } = Apps.SLMNG;
//
//         [Option(Template = "-e|--environment", Description = "Environment to deploy to.", ValueName = "environment-name")]
//         public Environments? Environment { get; set; } = Environments.Development;
//
//         private void OnExecute(CommandLineApplication app)
//         {
//             Console.WriteLine("Running...");
//             app.ValueParsers.Add(new EnvironmentsValueParser());
//             Console.WriteLine($"Deploying app \"{App}\" to environment \"{Environment}\"");
//         }
//     }
//
//     public enum Apps
//     {
//         SLMNG = 1,
//         SLREQ = 2,
//         SLORD = 3,
//         SLINV = 4,
//     }
//
//     public enum Environments
//     {
//         Development = 1,
//         Staging = 2,
//         Production = 3,
//     }
//
//     public class EnvironmentsValueParser : IValueParser<Environments?>
//     {
//         object? IValueParser.Parse(string? argName,
//             string? value,
//             CultureInfo culture)
//         {
//             return Parse(argName, value, culture);
//         }
//
//         public Environments? Parse(string? argName,
//             string? value,
//             CultureInfo culture)
//         {
//             Console.WriteLine("Converting...");
//             if (argName != nameof(Deploy.Environment))
//             {
//                 return null;
//             }
//
//             return value switch
//             {
//                 "dev" or "Development" => Environments.Development,
//                 _ => null
//             };
//         }
//
//         public Type TargetType { get; } = typeof(Environments?);
//     }
// }

