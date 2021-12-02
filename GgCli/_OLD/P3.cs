// // Copyright (c) Nate McMaster.
// // Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//
// using System;
// using System.Threading.Tasks;
// using McMaster.Extensions.CommandLineUtils;
//
// /// <summary>
// ///     You can use async with attributes by calling <see cref="CommandLineApplication.ExecuteAsync" />
// ///     and using a method named "OnExecuteAsync" on your app type.
// /// </summary>
// public class AsyncWithAttributes
// {
//     [Option(ShortName = "n")] public int Count { get; }
//
//     [Option(Description = "The subject")] public string Subject { get; }
//
//     public static Task<int> Main3(string[] args)
//     {
//         return CommandLineApplication.ExecuteAsync<AsyncWithAttributes>(args);
//     }
//
//     private async Task OnExecuteAsync()
//     {
//         var subject = Subject ?? "world";
//
//         // This pause here is just for indication that some awaitable operation could happens here.
//         await Task.Delay(5000);
//
//         for (var i = 0; i < Count; i++) Console.WriteLine($"Hello {subject}!");
//     }
// }

