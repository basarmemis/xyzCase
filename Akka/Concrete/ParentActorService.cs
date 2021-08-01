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
using Case.Services;
using Microsoft.Extensions.Hosting;

namespace Case.Akka.Concrete
{
    public class ParentActorService : IParentActorService, IHostedService
    {
        private ActorSystem _actorSystem;
        public IActorRef parentActor { get; private set; }
        private Props parentActorProps;
        private readonly IServiceProvider _sp;
        private readonly SymbolsResponseObjectClient _symbolResponseObjectClient;
        private Config hocon;
        private BootstrapSetup bootstrap;
        private DependencyResolverSetup di;

        public ParentActorService(IServiceProvider sp, SymbolsResponseObjectClient symbolResponseObjectClient)
        {
            _sp = sp;
            _symbolResponseObjectClient = symbolResponseObjectClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            hocon = ConfigurationFactory.ParseString(await File.ReadAllTextAsync("app.conf", cancellationToken));
            bootstrap = BootstrapSetup.Create().WithConfig(hocon);
            di = DependencyResolverSetup.Create(_sp);
            _actorSystem = ActorSystem.Create("ParentActorSystem", bootstrap.And(di));
            parentActorProps = DependencyResolver.For(_actorSystem).Props<ParentActor>();
            parentActor = _actorSystem.ActorOf(parentActorProps.WithRouter(FromConfig.Instance), "parentactor");
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
        public async Task StartProcess()
        {
            await Task.Delay(0);
            parentActor.Tell("Start");
        }
    }
}