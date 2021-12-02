using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GgCli.ValueParsers;
using McMaster.Extensions.CommandLineUtils;
using UnconstrainedMelody;

namespace GgCli.Commands
{
    [Command(
        Description = "Does app deployments",
        Name = "d",
        FullName = "deploy")]
    public class Deploy
    {
        [Argument(
            0,
            Name = "app",
            Description = "App to deploy")]
        [Required]
        // [Option(
        //     Template = "-a|--app",
        //     Description = "App to deploy",
        //     ValueName = "application-name")]
        private Apps App { get; } = Apps.SalesManagement;

        [Option(
            Template = "-e|--environment",
            Description = "Environment to deploy to.",
            ValueName = "environment-name")]
        private Environments Environment { get; } = Environments.Development;

        private void OnExecute(
            CommandLineApplication app)
        {
            var validationResults = ValidateEnumArgumentsOrOptions();
            if (!validationResults.IsValid)
            {
                foreach (var error in validationResults.Errors) Console.WriteLine(error);

                return;
            }

            Console.WriteLine("Running deploy command...");
            // Console.WriteLine($"App: {App}, Environment: {Environment}");
            Console.WriteLine($"Deploying \"{App.GetDescription()}\" to \"{Environment.GetDescription()}\"");
        }

        private (bool IsValid, List<string> Errors) ValidateEnumArgumentsOrOptions()
        {
            var errors = new List<string>();
            if (!Enum.GetValues<Apps>().Contains(App))
                errors.Add(
                    "Provided 'app' parameter is not allowed." +
                    $" Allowed values are : {string.Join(", ", Enum.GetNames(typeof(Apps)))}"
                );

            if (!Enum.GetValues<Environments>().Contains(Environment))
                errors.Add(
                    "Provided 'environment' parameter is not allowed." +
                    $" Allowed values are : {string.Join(", ", Enum.GetNames(typeof(Environments)))}"
                );

            return (!errors.Any(), errors);
        }
    }
}