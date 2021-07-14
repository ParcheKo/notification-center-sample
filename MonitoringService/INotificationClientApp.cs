using System.Threading.Tasks;

namespace MonitoringService
{
    public interface INotificationClientApp
    {
        Task ReceiveSimpleMessage(string message);
        Task ReceiveFileCreatedEvent(string message);
    }
}