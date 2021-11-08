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

namespace ASP.NETCore_configuration_default_2021_11nov_06
{
    public class Startup
    {
        IConfiguration config;
        public Startup(IConfiguration _config)  // ctor(_config) - actually app already have
        {                                       //   an config instance as service in its service
                                                //   collection. Yes, IConfiguration is a service
                                                //   type (and someone else is its implementation)
                                                //
                                                //   IConfiguration is the only service which
                                                //   ctor of Startup can receive
            config = _config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use((context, next) =>
            {
                context.Response.WriteAsync($"firstname: {config["firstname"]}, " +
                    $"age: {config["age"]}\n");
                                    // config["firstname"] - we can pass these key-value pairs
                                    //   throught command line (    firstname=Kenuchi...    )
                                    //   
                                    //   We can do it because service CommandLine is one of the
                                    //     default services of this app (added throught
                                    //         services.AddCommandLine()    ). This service adds
                                    //     additional provider to our global config
                return next.Invoke();
            });
            app.Use((context, next) =>
            {
                context.Response.WriteAsync($"your default editor is {config["EDITOR"]}\n");
                return Task.CompletedTask;
            });
        }
    }
}
