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

namespace ASP.NETCore_configuration_additional_files_2021_11nov_07
{
    public class Startup
    {
        IConfiguration config;
        public Startup(IConfiguration _config)
        {
            //config = new ConfigurationBuilder().AddConfiguration(_config)
            //    .AddJsonFile("./somedata.json").Build();
                                                // .AddConfiguration().AddJsonFile() - if our
                                                //   ./somedata.json sets keys that already were
                                                //   in default config, it override them
                                                //
                                                //       Actually not always. If xml service defined
                                                //         previously than json service (it's so
                                                //         by default), than xml file overrides
                                                //         all next ensuing json files
                                                //
                                                //   If there is no such file nearby to output .dll,
                                                //     you will catch an exception
                                                //
            config = new ConfigurationBuilder().AddConfiguration(_config)
                .AddJsonFile("./somedata.json").AddXmlFile("./otherdata.xml").Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConfiguration>(provider => config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

//          app.Use((context, next) =>
//          {
//              context.Response.WriteAsync( $"<p style='color:{config["color"]};'>" +
//                  $"{config["text"]}</p>");
//              return Task.CompletedTask;
//          });
            app.Use((context, next) =>
            {
                context.Response.WriteAsync($"<p style='color:{config["color"]};'>" +
                    $"{config["text:description:data"]}</p>\n");
                IConfigurationSection section = config.GetSection("text");
                                            // config.GetSection() - if it really would be a
                                            //   section, we could got it with config["text"]
                                            //   operator
                                            //
                                            //       We also could go deeper with it. For example,
                                            //         config["text:description:data"]
                                            //
                context.Response.WriteAsync($"section value is {section.Value}\n");
                                            // section.Value - it's "Ah! Leah!"
                return Task.CompletedTask;
            });
        }
    }
}
