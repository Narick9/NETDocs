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
//      public void Configure_(IApplicationBuilder app, IWebHostEnvironment env)
//      {
//          env.EnvironmentName = "Production";
//          if (env.IsDevelopment())
//          {
//              app.UseDeveloperExceptionPage();  // app.UseDeveloperExceptionPage() - it enables
//                                                //   good looking page with occured exceptions
//                                                //   that were not catched
//                                                //
//                                                //   It also has a overload where you can pass
//                                                //     an UseDeveloperExceptionPageOptions class
//                                                //     instance
//                                                //
//                                                //   Without this middleware component, user
//                                                //     will receive HTTP ERROR 500
//                                                //
//          }
//
//          app.Use((context, next) =>
//          {
//              //context.Response.WriteAsync("Hello\n");  // ...WriteAsync() - but, for some
//              return next.Invoke();                      //   reasons, 
//          });
//          app.Run(CalculationPage);
//      }
        public Task CalculationPage(HttpContext context)
        {
            int a = 6;
            int b = 0;
            return context.Response.WriteAsync($"{a} / {b} = {a / b}");
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            env.EnvironmentName = "Production";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");  // app.UseExceptionHandler() - the middleware
                                                    //   catches exceptions, logs them, and
                                                    //   re-execute the request in an altrenate
                                                    //   pipeline. The request will nob be
                                                    //   re-executed if the request has already
                                                    //   started
                                                    //
                                                    //   It has an overload with
                                                    //     ExceptionHandlerOptions parameter
            }

            app.Map("/error", ErrorPage);
            app.Run(CalculationPage);
        }
        public void ErrorPage(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                return context.Response.WriteAsync("An exception has occured!");
            });
        }
    }
}
