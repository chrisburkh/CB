using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Photo.Api.Messaging.Consumer;
using Photo.Api.Repository;
using Photo.Api.Service;

namespace Photo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Autofac.IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // services.AddScoped<IPersonsRepository>(c => new PersonsRepository(Configuration["RavendbUrl"]));

            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IPhotoRepository, PhotoRepository>();


            var builder = new ContainerBuilder();

            // register a specific consumer
            builder.RegisterType<PhotoDeleteConsumer>();

            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri("rabbitmq://rabbitmq/"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // https://stackoverflow.com/questions/39573721/disable-round-robin-pattern-and-use-fanout-on-masstransit
                    cfg.ReceiveEndpoint(host, "dotnetgigs" + Guid.NewGuid().ToString(), e =>
                    {
                        e.LoadFrom(context);
                        //e.Consumer<ApplicantAppliedConsumer>();
                    });
                });

                return busControl;
            })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();


            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
