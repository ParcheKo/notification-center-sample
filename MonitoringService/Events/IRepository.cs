using System.Threading.Tasks;

namespace MonitoringService.Events
{
    public interface IRepository<T> : IReadOnlyRepository<T>
        where T : Entity
    {
        Task<T> Add(
            T entity);
    }
}