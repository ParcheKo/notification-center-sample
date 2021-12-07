using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GgCli.ValueParsers;
using McMaster.Extensions.CommandLineUtils;
using UnconstrainedMelody;

namespace GgCli.Commands
{
    public static class Deploy
    {
        public const string Name = "deploy";
        public const string Alias = "d";

        public static CommandArgument<Apps?> BuildAppArgument()
        {
            var app = new CommandArgument<Apps?>(
                new AppsValueParser())
            {
                Name = AppConstants.Name,
                Description = AppConstants.Description
            };
            if (AppConstants.IsRequired) app.IsRequired();

            // app.DefaultValue = AppConstants.DefaultValue;
            return app;
        }

        public static CommandOption<Environments?> BuildEnvironmentOption()
        {
            var environment = new CommandOption<Environments?>(
                new EnvironmentsValueParser(),
                EnvironmentConstants.Template,
                EnvironmentConstants.OptionType)
            {
                Description = EnvironmentConstants.Description
            };
            if (EnvironmentConstants.IsRequired) environment.IsRequired();

            // environment.DefaultValue = EnvironmentConstants.DefaultValue;
            return environment;
        }

        public static CommandOption<SemVersion?> BuildVersionOption()
        {
            var version = new CommandOption<SemVersion?>(
                new SemVersionValueParser(),
                VersionConstants.Template,
                VersionConstants.OptionType)
            {
                Description = VersionConstants.Description
            };
            if (VersionConstants.IsRequired) version.IsRequired();

            // version.DefaultValue = VersionConstants.DefaultValue;
            return version;
        }

        // public class MyClass : IValidator
        // {
        //     public ValidationResult? GetValidationResult(
        //         CommandOption option,
        //         ValidationContext context)
        //     {
        //         
        //     }
        //
        //     public ValidationResult? GetValidationResult(
        //         CommandArgument argument,
        //         ValidationContext context)
        //     {
        //         throw new NotImplementedException();
        //     }
        // }

        public static async Task<int> OnExecuteAsync(
            CancellationToken cancellationToken,
            Apps app,
            Environments environment,
            SemVersion version)
        {
            Console.WriteLine(
                $"Deploying \"{app.GetDescription()}\" " +
                $"to \"{environment.GetDescription()}\" " +
                $"from version \"{version}\""
            );
            return 0;
            // var validationResults = ValidateEnumArgumentsOrOptions();
            // if (!validationResults.IsValid)
            // {
            //     foreach (var error in validationResults.Errors) Console.WriteLine(error);
            //
            //     return 1;
            // }
            //
            // // Console.WriteLine($"App: {App}, Environment: {Environment}");
            // Console.WriteLine(
            //     $"Deploying \"{App.GetDescription()}\" " +
            //     $"to \"{Environment.GetDescription()}\"" +
            //     $"from version \"{1}\""
            // );
        }

        public static (bool IsValid, List<string> Errors) ValidateEnumArgumentsOrOptions(
            Apps app,
            Environments environment)
        {
            var errors = new List<string>();
            if (!Enum.GetValues<Apps>().Contains(app))
                errors.Add(
                    "Provided 'app' parameter is not allowed." +
                    $" Allowed values are : {string.Join(", ", Enum.GetNames(typeof(Apps)))}"
                );

            if (!Enum.GetValues<Environments>().Contains(environment))
                errors.Add(
                    "Provided 'environment' parameter is not allowed." +
                    $" Allowed values are : {string.Join(", ", Enum.GetNames(typeof(Environments)))}"
                );

            return (!errors.Any(), errors);
        }

        private static class AppConstants
        {
            // public static int Order = 0;
            public const string Name = "app";
            public const string Description = "App to deploy";
            public const bool IsRequired = true;
            public const Apps DefaultValue = Apps.SalesManagement;
        }

        private static class EnvironmentConstants
        {
            public const string Template = "-e|--environment <ENVIRONMENT>";
            public const string Description = "Environment to deploy to";
            public const CommandOptionType OptionType = CommandOptionType.SingleValue;
            public const bool IsRequired = true;
            public const Environments DefaultValue = Environments.Development;
        }

        private static class VersionConstants
        {
            public const string Template = "-v|--version <VERSION>";
            public const string Description = "Version of app to deploy (in the form of Semantic Versions)";
            public const CommandOptionType OptionType = CommandOptionType.SingleValue;
            public const bool IsRequired = true;
            public static readonly SemVersion DefaultValue = SemVersion.From("1.0.0");
        }
    }
}