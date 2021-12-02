using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringService.Persistance
{
    public interface IReadOnlyRepository<T>
        where T : Entity
    {
        Task<T> GetById(
            Guid id);

        Task<IEnumerable<T>> GetAll();
    }
}