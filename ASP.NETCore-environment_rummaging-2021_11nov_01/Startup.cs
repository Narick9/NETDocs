using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_environment_rummaging_2021_11nov_01
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use((context, next) =>
            {
                context.Response.WriteAsync(
                    $"The \"{env.ApplicationName}\" is in {env.EnvironmentName}!\n");
                return next.Invoke();
            });

            if (env.EnvironmentName == "Test")
            {
                app.Use((context, next) =>
                {
                    context.Response.WriteAsync("\nYeah, you are definetly in testing right now\n");
                    return next.Invoke();
                });
            }

            app.Run((context) =>
            {
                return context.Response.WriteAsync("\nIt's final middleware component");
            });
        }
    }
}
