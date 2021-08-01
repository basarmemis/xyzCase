using System.Threading;
using System.Threading.Tasks;
using Case.Model;

namespace Case.Akka.Interface
{
    public interface IChildActorService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);

        void CalculateAvarage(Symbol symbol);
    }
}