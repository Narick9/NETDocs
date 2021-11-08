using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace ASP.NETCore_configuration_2021_11nov_06
{
    public class Startup
    {
        static IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string>()
            {
                ["firstname"] = "Tom",
                ["age"] = "31",
            }).Build();
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }
        bool switcher = false;
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

//          bool switcher = false;
//          app.Map("/change", app =>
//          {
//              if (switcher)
//              {
//                  Startup.config["firstname"] = "Alice";
//                  Startup.config["age"] = "31";
//                  app.Run(context => context.Response.WriteAsync("Changed"));
//              }
//              else
//              {
//                  switcher = true;
//                  app.Run(context => context.Response.WriteAsync("Switcher is switched"));
//              }
//          });         // Actually you can't do this switcher like this because method Configure()
//                      //   (and logic in this app => { ... } delegate) processed only once
//                      //
//                      // Instead it you should make full-fledged middleware class (such as
//                      //   ChangerMiddleware)

            app.UseMiddleware(typeof(ChangerMiddleware));
            app.Run(context =>
            {
                return context.Response.WriteAsync($"firstname is {Startup.config["firstname"]}");
            });
        }
        class ChangerMiddleware
        {
            RequestDelegate _next;
            public ChangerMiddleware(RequestDelegate next)
            {
                _next = next;
            }
            public Task Invoke(HttpContext context)
            {
                if (context.Request.Path == "/change")
                {
                    Startup.config["firstname"] = "Alice";
                    Startup.config["age"] = "31";
                    context.Response.WriteAsync("Changed");
                    return Task.CompletedTask;
                }
                else
                    return _next.Invoke(context);
            }
        }
    }
}
