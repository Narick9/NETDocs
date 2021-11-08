using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_multipage_site_2021_10oct_29
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

            app.UseRouting();

            //My customization:

            app.Map("/about", AboutPage);              // app.Map("/about", ...) - forkes the
                                                       //   request pipeline to new branch
                                                       //   (new branch is configured in
                                                       //   AboutPage())
            app.Map("/spreadsheet", SpreadsheetPage);  // AboutPage(), SpreadsheetPage(), ... - I
            app.Map("/fork", ForkPage);                //   didn't do it asyncronous there
            app.Map("/detailedInfo", DetailedInfoPage);
            app.MapWhen((context) =>
                context.Request.Path == new PathString("/ZatoIno") &&
                context.Request.Query.ContainsKey("pet") &&
                context.Request.Query["pet"] == "spot", ZatoInoPage);

            app.Run((context) =>  // app.Run() - there is also nothing about asynchrony
            {
                return context.Response.WriteAsync("There is no such page");
            });
        }
        static void AboutPage(IApplicationBuilder app)
        {
            app.Run((context) =>  //****app.Run() - does it add the lambda to middleware for awhile
            {                     //    or for permanent?
                return context.Response.WriteAsync("\"About\" page");
            });
        }
        static void SpreadsheetPage(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                context.Response.WriteAsync("Number|\tPower of 2\n");
                context.Response.WriteAsync("------------------\n");
                for (int i = 1; i < 10; i++)
                {
                    context.Response.WriteAsync($"{i} * {i} | {i*i}\n");
                }
                return context.Response.WriteAsync("\ndone");
            });
        }
        static void ForkPage(IApplicationBuilder app)
        {
            app.Map("/wayOfMagistrate", (app) =>
            {
                app.Run((context) =>
                {
                    return context.Response.WriteAsync("Welcome, Ando");
                });
            });
            app.Map("/wayOfSamurai", (app) =>
            {
                app.Run((context) =>
                {
                    return context.Response.WriteAsync("Welcome, Usagi");
                });
            });
            app.Run((context) =>
            {
                return context.Response.WriteAsync(
                    "There are two ways:\n" +
                    "/wayOfMagistrate\n" +
                    "/wayOfSamurai\n\n");
            });
        }
        static void DetailedInfoPage(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                return context.Response.WriteAsync($"_{context.Request.QueryString.ToString()}_");
            });  // context.Request.QueryString --- it will content something only if you added
                 //   extra info in query string
                 //
                 //       For example, with query as
                 //
                 //          localhost:5000/detailedinfo?421    
                 //
                 //       context.Request.QueryString will contains "?421"
                 //
        }
        static void ZatoInoPage(IApplicationBuilder app)
        {
            app.Run((context) =>
            {
                return context.Response.WriteAsync("You've deserved your rest, Ino.\n" +
                                                   "Rest in peace, Spot...");
            });
        }
    }
}
