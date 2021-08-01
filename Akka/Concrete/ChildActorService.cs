using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;
using Akka.Routing;
using Case.Akka.Actors;
using Case.Akka.Interface;
using Case.Model;
using Microsoft.Extensions.Hosting;

namespace Case.Akka.Concrete
{
    public class ChildActorService : IChildActorService, IHostedService
    {
        private ActorSystem _actorSystem;
        public IActorRef childActor { get; private set; }
        private Props childActorProps;
        private readonly IServiceProvider _sp;
        private Config hocon;
        private BootstrapSetup bootstrap;
        private DependencyResolverSetup di;

        public ChildActorService(IServiceProvider sp)
        {
            _sp = sp;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            hocon = ConfigurationFactory.ParseString(await File.ReadAllTextAsync("app2.conf", cancellationToken));
            bootstrap = BootstrapSetup.Create().WithConfig(hocon);
            di = DependencyResolverSetup.Create(_sp);
            _actorSystem = ActorSystem.Create("ChildActorSystem", bootstrap.And(di));
            childActorProps = DependencyResolver.For(_actorSystem).Props<ChildActor>();
            childActor = _actorSystem.ActorOf(childActorProps.WithRouter(FromConfig.Instance), "childactor");


            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }

        public void CalculateAvarage(Symbol symbol)
        {
            childActor.Tell(symbol);
        }
    }
}