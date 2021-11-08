using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_http_errors_2021_11nov_03
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
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();  // app.UseStatusCodePages() - the middleware gives default
                                       //   response handler for HTTP errors between 400 and 599
                                       //
                                       // Here we can handle only "localhost:5000/hello" request.
                                       //   To all other request paths this middleware will
                                       //   send a string    "Status Code: 404; Not Found"

//          app.UseStatusCodePages("text/plain", "Your status code: {0}");
                                       // app.UseStatusCodePages(...) - with this overload you can
                                       //   configure your wished output string
                                       //
                                       //   "text/plain" - MIME-type of your message
                                       //

                                       // Also the component method has an overload with
                                       //   StatusCodePagesOptions class parameter

//          app.UseStatusCodePagesWithRedirects("/error?code={0}");
                                              // app.UseStatusCodePagesWithRedirects(...) - if
                                              //   if HTTP error has occured, the middleware
                                              //   (the same StatusCodePages) redirects that
                                              //   request to given path
                                              //
                                              //       This {0} will be replaced by HTTP error code
                                              //

            app.UseStatusCodePagesWithReExecute("/error", "?code={0}");
                                              // app.UseStatusCodePagesWithRedirects(...) -
                                              //   does the same as
                                              //   app.UseStatusCodePagesWithRedirects(), but
                                              //   with re-executing the request

            app.Map("/hello", (app) =>
            {
                app.Run(context => context.Response.WriteAsync("Hello"));
            });
            app.MapWhen((context) => context.Request.Path == "/error" &&
                                     context.Request.Query["code"] == "404", (app) =>
            {
                app.Run(context => context.Response.WriteAsync("Sorry, this page was not found"));
            });
        }
        public void Configure_(IApplicationBuilder app)
        {
            // There is also an other way to handle HTTP errors - web.config file. It was popular
            //   way there, in ASP.NET Classic time. Now, during the era of ASP.NET Core
        }
    }
}
