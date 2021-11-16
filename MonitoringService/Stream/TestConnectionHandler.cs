﻿// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Connections;
// using Microsoft.Extensions.Logging;
//
// namespace MonitoringService.Stream
// {
//     public class TestConnectionHandler : ConnectionHandler
//     {
//         private readonly ILogger<TestConnectionHandler> _logger;
//
//         public TestConnectionHandler(ILogger<TestConnectionHandler> logger)
//         {
//             _logger = logger;
//         }
//
//         public override async Task OnConnectedAsync(ConnectionContext connection)
//         {
//             _logger.LogInformation("{ConnectionId} connected", connection.ConnectionId);
//
//             while (true)
//             {
//                 var result = await connection.Transport.Input.ReadAsync();
//                 var buffer = result.Buffer;
//
//                 foreach (var segment in buffer)
//                 {
//                     await connection.Transport.Output.WriteAsync(segment);
//                 }
//
//                 if (result.IsCompleted)
//                 {
//                     break;
//                 }
//
//                 connection.Transport.Input.AdvanceTo(buffer.End);
//             }
//
//             _logger.LogInformation("{ConnectionId} disconnected", connection.ConnectionId);
//         }
//     }
// }

