using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_cookies_2021_11nov_09
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
            app.UseDeveloperExceptionPage();

            app.Run(context =>
            {
                if (context.Request.Cookies.ContainsKey("name"))
                {
                                                      // ContainsKey("name") - for the first time
                                                      //   there should not be such cookie. But
                                                      //   by next time it should be
                    string name = context.Request.Cookies["name"];
                    return context.Response.WriteAsync($"Hello, {name}");
                }
                else
                {
                    context.Response.Cookies.Append("name", "Thomas");
                                                      // Append("name", ..) - saves this element
                                                      //   on user side
                    return context.Response.WriteAsync("Hello world!");
                }
            });
        }
    }
}
