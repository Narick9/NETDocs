using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_authendefication_2021_10oct_31
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
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.Use(Authentication);
            app.Run((context) =>
            {
                return context.Response.WriteAsync("Home page\n");
            });
        }



        private class ErrorHandlingMiddleware
        {
            private RequestDelegate next;
            public ErrorHandlingMiddleware(RequestDelegate _next)
            {
                next = _next;
            }
            public Task Invoke(HttpContext context)
            {
                next.Invoke(context);                    // next.Invoke(); ... - as you can see,
                                                         //   we start this method and after
                                                         //   check its trace
                if (context.Response.StatusCode == 403)
                {
                    return context.Response.WriteAsync("Access denied\n");
                }
                else if (context.Response.StatusCode == 404)
                {
                    return context.Response.WriteAsync("Page is not found");
                }
                return Task.CompletedTask;
            }
        }
        private Task Authentication(HttpContext context, Func<Task> next)
        {                                   // Authentication() - it is not a good pattern
                                            //   (I mean using method instead ..Middleware class),
                                            //   (I've never seen that before) , but it also
                                            //   looks excellent
            if (context.Request.Query["login"] == "kitsune" &&
                context.Request.Query["password"] == "1616ge")
            {
                return next.Invoke();
            }
            else  // else - as you can see, async method can not return something
            {
                context.Response.StatusCode = 403;
                return default;
            }
        }
    }
}
