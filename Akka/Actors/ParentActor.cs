using System;
using System.Threading.Tasks;
using Akka.Actor;
using Case.Akka.Interface;
using Case.Model;
using Case.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Case.Akka.Actors
{
    public class ParentActor : ReceiveActor
    {
        private readonly IChildActorService childActorService;
        private readonly IServiceScope _scope;
        private readonly IDummyLogger _dummylogger;
        private readonly SymbolsResponseObjectClient _symbolResponseObjectClient;

        public ParentActor(IServiceProvider sp, SymbolsResponseObjectClient symbolResponseObjectClient)
        {
            _scope = sp.CreateScope();
            _dummylogger = _scope.ServiceProvider.GetRequiredService<IDummyLogger>();
            childActorService = _scope.ServiceProvider.GetRequiredService<IChildActorService>();
            _symbolResponseObjectClient = symbolResponseObjectClient;
            this.ReceiveAsync<string>(StartChildActors);
        }

        public async Task StartChildActors(string message)
        {
            _dummylogger.PrintLog(message);
            try
            {
                SymbolsResponseObject _symbolsResponseObject = await _symbolResponseObjectClient.GetCurrentResponse();


                if (_symbolsResponseObject != null)
                {
                    childActorService.CalculateAvarage(_symbolsResponseObject.AAPL);
                    childActorService.CalculateAvarage(_symbolsResponseObject.EURUSD);
                    childActorService.CalculateAvarage(_symbolsResponseObject.MSFT);
                    childActorService.CalculateAvarage(_symbolsResponseObject.NKE);
                    childActorService.CalculateAvarage(_symbolsResponseObject.SBUX);
                }
                else
                {
                    Console.WriteLine($"Response is null");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            await Task.Delay(5000);
            Self.Tell("Recursive");
            GC.Collect();
        }

        protected override void PreStart()
        {
            _dummylogger.PrintLog("ParentActor Started");
        }
        protected override void PostStop()
        {
            _dummylogger.PrintLog("ParentActor Stopped");
            _scope.Dispose();
        }
    }
}