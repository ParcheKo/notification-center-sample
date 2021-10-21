using System.Threading.Tasks;
using MonitoringService.Events;

namespace MonitoringService.Stream
{
    public interface INotificationHubClient
    {
        Task ReceiveSimpleMessage(string message);
        Task ReceiveAppPublishedMessage(AppPublished message);
    }
}