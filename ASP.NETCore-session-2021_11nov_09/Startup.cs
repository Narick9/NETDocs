using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_session_2021_11nov_09
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseSession();
            app.Run(context =>
            {
                if (context.Session.Keys.Contains("name"))
                {
                            // context.Session - it's like cookie, but on the server. Session
                            //   can store user's info while he browses the site. Usually session
                            //   live for 20 min after last user request
                    return context.Response.WriteAsync(
                        $"Hello {context.Session.GetString("name")}");
                }
                else
                {
                    context.Session.SetString("name", "Thomas");
                    return context.Response.WriteAsync("Hello world!");
                }
            });
        }
    }
}
