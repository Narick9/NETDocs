using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_errors_2021_11nov_03
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // app.UseDeveloperExceptionPage() - it enables
                                                  //   good looking page with occured exceptions
                                                  //   that were not catched
                app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions()
                {

                })
            }

            app.Use((context, next) =>
            {
                //context.Response.WriteAsync("Hello\n");  // ...WriteAsync() - but, for some
                return next.Invoke();                      //   reasons, 
            });
            app.Run((context) =>
            {
                int a = 6;
                int b = 0;
                return context.Response.WriteAsync($"{a} / {b} = {a / b}");
            });
        }
    }
}
