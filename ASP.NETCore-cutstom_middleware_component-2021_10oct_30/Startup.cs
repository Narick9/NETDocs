using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_cutstom_middleware_component_2021_10oct_30
{
    public class Startup
    {
        private class FatherMiddleware
        {
            RequestDelegate next;
            public FatherMiddleware(RequestDelegate _next)
            {
                next = _next;
            }
            public async Task InvokeAsync(HttpContext context)
            {
                if (context.Request.Query["father"].ToString().ToLower() == "murakami")
                {
                    await next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("You are not Gennosuke");
                }
            }
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<FatherMiddleware>();  // app.UseMiddleware<>() - you also can pass
                                                    //   to this method params that will sent to
                                                    //   ctor of FatherMiddleware
                                                    // As metanit says, it is not rare they
                                                    //   make an extension method for adding
                                                    //   custom components (for example, we
                                                    //   could make    app.UseFather()    )
  
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello, Gennosuke");
            });
        }
//      public void Configure_(IApplicationBuilder app)  // Configure_() - actually this method
//      {                                                //   does the same, but without
//                                                       //   building custom middleware component
//          app.Use(async (context, next) =>
//          {
//              if (context.Request.Query["father"] == "murakami")
//              {
//                  await next.Invoke();
//              }
//              else
//              {
//                  await context.Response.WriteAsync("You are not Gennosuke");
//              }
//          });
//
//          app.Run(async (context) =>
//          {
//              await context.Response.WriteAsync("Welcome, Gennosuke");
//          });
//      }
    }
}
