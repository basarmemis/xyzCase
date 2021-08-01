using System;
using System.Threading.Tasks;
using Akka.Actor;
using Case.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Case.Akka.Actors
{
    public class ChildActor : ReceiveActor
    {

        private readonly IServiceScope _scope;
        private readonly IDummyLogger _dummylogger;

        public ChildActor(IServiceProvider sp)
        {
            _scope = sp.CreateScope();
            _dummylogger = _scope.ServiceProvider.GetRequiredService<IDummyLogger>();
            this.ReceiveAsync<Symbol>(HandleAsync);
        }

        public async Task HandleAsync(Symbol symbol)
        {
            double total = 0;
            foreach (Value x in symbol.values)
            {
                total = total + Convert.ToDouble(x.close);
            }
            total = total / symbol.values.Count;
            await Task.Delay(0);
            _dummylogger.PrintLog($"Avarage Value Of {symbol.meta.symbol} = {total}");
            //GC.Collect();
        }

        protected override void PreStart()
        {
            _dummylogger.PrintLog("ChildActor Started");
        }
        protected override void PostStop()
        {
            _dummylogger.PrintLog("CildActor Stopped");
            _scope.Dispose();
        }
    }
}