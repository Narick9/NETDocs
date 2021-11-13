using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_logging_configuration_2021_11nov_12
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

            // By default project has ./appsettings.json. There already are some
            //   logging configuration
            //
            //       {
            //         "Logging": {       // contains general logging config
            //           "LogLevel": {    // sets minimal log level for three categories
            //             "Default": "Information",  // by default will be logging all messages
            //                                        //   of level Information
            //             "Microsoft": "Warning",    // from "Microsoft" category will be logging
            //                                        //   all messages of level Warning
            //             "Microsoft.Hosting.Lifetime": "Information"
            //           },                           // from "Microsoft.Hosting.Lifetime" category
            //                                        //   will be logging all messages of level
            //                                        //   Information
            //                                        //****what is category here?
            //           
            //           "Console": {      // contain Console provider logging info
            //             "LogLevel": {
            //               "Default": "Information",
            //               "Microsoft": "Warning"
            //             }
            //           },
            //           "LogLevel": {    // general rule for all providers
            //             "Default": "Error"
            //           },
            //         },
            //       }
            //
            //   By the way, you can adjust filters also throught LoggerFactory
        }
    }
}
