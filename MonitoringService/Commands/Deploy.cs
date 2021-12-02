using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MonitoringService.Commands
{
    public class Deploy : IRequest
    {
        public class Deployer : IRequestHandler<Deploy>
        {
            public async Task<Unit> Handle(
                Deploy request,
                CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}