// public static async Task<int> Main_old(
//     string[] args)
// {
//     Console.WriteLine("Running command-line application!");
//     // return CommandLineApplication.ExecuteAsync<Cli>(args);
//
//     var app = new CommandLineApplication<Program>();
//     app.ValueParsers.AddRange(
//         new List<IValueParser>()
//         {
//             new EnvironmentsValueParser(),
//             new AppsValueParser()
//         });
//     // return app.ExecuteAsync(args);
//     return await new HostBuilder()
//         .ConfigureLogging(
//             (
//                 context,
//                 builder) =>
//             {
//                 builder.AddConsole();
//             })
//         .ConfigureServices(
//             (
//                 context,
//                 services) =>
//             {
//                 services
//                     .AddSingleton<IConsole>(PhysicalConsole.Singleton);
//             })
//         .RunCommandLineApplicationAsync<Program>(args);
// }


