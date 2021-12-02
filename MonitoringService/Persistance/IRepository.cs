using System.Threading.Tasks;

namespace MonitoringService.Persistance
{
    public interface IRepository<T> : IReadOnlyRepository<T>
        where T : Entity
    {
        Task<T> Add(
            T entity);
    }
}