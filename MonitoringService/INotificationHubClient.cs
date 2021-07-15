using System.Threading.Tasks;

namespace MonitoringService
{
    public interface INotificationHubClient
    {
        Task ReceiveSimpleMessage(string message);
        Task ReceiveAppPublishedMessage(AppPublished message);
    }
}