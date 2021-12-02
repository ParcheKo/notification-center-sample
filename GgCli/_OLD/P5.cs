// using System;
// using GgCli;
// using McMaster.Extensions.CommandLineUtils;
//
// var app = new CommandLineApplication();
//
// app.ValueParsers.Add(new EnvironmentsValueParser());
// app.HelpOption();
//
// var subject = app.Option("-s|--subject <SUBJECT>", "The subject", CommandOptionType.SingleValue);
// subject.DefaultValue = "world";
//
// var repeat = app.Option<int>("-n|--count <N>", "Repeat", CommandOptionType.SingleValue);
// var env = app.Option<Environments>("-e|--env <E>", "Environment", CommandOptionType.SingleValue);
// env.DefaultValue = Environments.Development;
//
// app.OnExecute(() =>
// {
//     for (var i = 0; i < repeat.ParsedValue; i++)
//     {
//         Console.WriteLine($"Hello {subject.Value()}!");
//         Console.WriteLine($"You provided environment \"{env.Value()}\"!");
//     }
// });
//
// return app.Execute(new string[] {"-e", "dev", "-n", "1"});

