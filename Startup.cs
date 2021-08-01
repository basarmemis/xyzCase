using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Case.Akka;
using Case.Akka.Concrete;
using Case.Akka.Interface;
using Case.Model;
using Case.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Case
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Case", Version = "v1" });
            });





            services.AddScoped<IDummyLogger, DummyLogger>();
            services.AddHttpClient<SymbolsResponseObjectClient>();
            services.AddSingleton<IParentActorService, ParentActorService>();
            services.AddSingleton<IChildActorService, ChildActorService>();

            services.AddHostedService<ParentActorService>(sp => (ParentActorService)sp.GetRequiredService<IParentActorService>());
            services.AddHostedService<ChildActorService>(sp => (ChildActorService)sp.GetRequiredService<IChildActorService>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Case v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
