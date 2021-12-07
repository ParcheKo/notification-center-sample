using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GgCli.ValueParsers;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Validation;

namespace GgCli.Commands
{
    public class DeployCommandValidator : IValidator
    {
        public ValidationResult? GetValidationResult(
            CommandOption option,
            ValidationContext context)
        {
            switch (option)
            {
                case CommandOption<Environments?> environment:
                    if (environment.ParsedValue is null || !Enum.GetValues<Environments>().Contains(environment.ParsedValue!.Value))
                        return new ValidationResult(
                            "Provided 'environment' parameter is not allowed." +
                            $" Allowed values are : {string.Join(", ", Enum.GetNames(typeof(Environments)))}"
                        );
                    break;
                case CommandOption<SemVersion> version:
                    break;
                default:
                    return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }

        public ValidationResult? GetValidationResult(
            CommandArgument argument,
            ValidationContext context)
        {
            if (argument is CommandArgument<Apps?> app)
            {
                if (app.ParsedValue is null || !Enum.GetValues<Apps>().Contains(app.ParsedValue.Value))
                    return new ValidationResult(
                        "Provided 'app' parameter is not allowed." +
                        $" Allowed values are : {string.Join(", ", Enum.GetNames(typeof(Apps)))}");
                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
    }
}