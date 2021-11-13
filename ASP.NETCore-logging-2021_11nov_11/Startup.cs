using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ASP.NETCore_logging_2021_11nov_11
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
                                                    // ILogger<Startup> - this is one of these
                                                    //   default dependency injections. It's
                                                    //   default whole .NET log-machine
                                                    //
                                                    //   Startup - it's just a label. Your logs
                                                    //     will be like:
                                                    //         
                                                    //         info: myApp.Startup[0]
                                                    //             bla bla bla
                                                    //

            app.UseDeveloperExceptionPage();

            app.Run(context =>
            {
                logger.LogInformation("Current request looks like \"{0}\"", context.Request.Path);
                            // logger.LogInformation() - this extension method is one of the easiest
                            //   way to just put your log to console (where you started site)
                            //
                            //   Be very watchful! This log will hidden between standard
                            //     ASP.NET logs

                // There are also some other levels of logging:
                //
                // logger.LogTrace();
                // logger.LogDebug();
                // logger.LogInformation();
                // logger.LogWarning();
                // logger.LogError();
                // logger.LogCritical();
                // not log
                //

                return context.Response.WriteAsync("We logged!");
            });
        }
    }
}
