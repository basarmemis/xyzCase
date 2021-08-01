using System;
using Case.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Case.Akka.Interface;
using System.Threading.Tasks;

namespace Case.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Run : ControllerBase
    {
        //private ActorSystem _actorSystem;
        private readonly IServiceScope _scope;
        private readonly IServiceProvider _sp;
        private readonly ILogger<Run> _logger;
        private readonly SymbolsResponseObjectClient _symbolResponseObjectClient;
        private readonly IParentActorService parentActorService;

        public Run(ILogger<Run> logger, SymbolsResponseObjectClient symbolResponseObjectClient, IServiceProvider sp)
        {
            _logger = logger;
            _symbolResponseObjectClient = symbolResponseObjectClient;
            _sp = sp;
            _scope = sp.CreateScope();
            parentActorService = _scope.ServiceProvider.GetRequiredService<IParentActorService>();
        }

        [HttpGet]
        public async Task<string> Get()
        {
            await parentActorService.StartProcess();
            return "Request Loop Started";
        }
    }
}
