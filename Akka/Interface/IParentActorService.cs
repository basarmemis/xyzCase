using System.Threading;
using System.Threading.Tasks;

namespace Case.Akka.Interface
{
    public interface IParentActorService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        Task StartProcess();
    }
}