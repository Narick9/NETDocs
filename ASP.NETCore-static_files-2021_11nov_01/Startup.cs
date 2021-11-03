using System;
using System.Collections.Generic;
//using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;

namespace ASP.NETCore_static_files_2021_11nov_01
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure_(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();  // app.UseStaticFiles() - if user requests the static files
                                   //   (./wwwroot/index.html), this middleware component takes
                                   //   all request on itself and completes it

            app.Map("/other/History2.html", (app) =>  // app.Map("/other/History2.html", (app)..) -
            {                                         //   this reference won't work if server
                app.Run((context) =>                  //   finds such file (spoiler: it finds)
                {
                    return context.Response.WriteAsync(
                        "Static file is not found. Maybe you will add it?");
                });
            });
            app.Run((context) =>
            {
                string message = "You should to request static files.\n" +
                                 "There are:\n";
                if (env.WebRootPath.EndsWith("/wwwroot/"))
                {                                       // env.WebRootPath - we can choose the path
                                                        //   in Program class (Startup.cs)
                    message += "\t/index.html\n" +
                               "\t/other/History.txt\n";
                }
                else if (env.WebRootPath.EndsWith("/my_other_statics/"))
                {
                    message += "\t/other/History2.html";
                }
                return context.Response.WriteAsync(message);
            });
        }
        public void Configure__(IApplicationBuilder app)
        {
            app.UseDefaultFiles();  // app.UseDefaultFiles() - with this component, server finds
                                    //   and uses default files as root page (localhost:5000/)
                                    //
                                    //       Default files are:
                                    //             > index.htm
                                    //             > index.html
                                    //             > default.htm,
                                    //             > default.html
                                    //
                                    //  You also can see that this component should be added before
                                    //    app.UseStaticFiles();
            app.UseStaticFiles();
            app.Run((context) => context.Response.WriteAsync("You can't see this message"));


            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("hello.html");
            //app.UseDefaultFiles(options);  // options - we could also configure this instance
                                             //   with our own wishes and pass it to
                                             //   app.UseDefaultFiles()
            //app.UseStaticFiles();
        }
        public void Configure___(IApplicationBuilder app)
        {
            //app.UseDirectoryBrowser();  // app.UseDirectoryBrowser() - ASP.NET Core even has
            //                            //   default directory browsing tool (with an edible UI)
            //app.UseStaticFiles();
            //app.Run((context) => context.Response.WriteAsync("You can't see this message"));


            //app.UseDirectoryBrowser("/obj");  // app.UseDirectoryBrowser(..) - with it, directory
                                                //   browser could be used on localhost:5000/obj
                                                //   path
            //app.UseStaticFiles();


            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider("/home"),
                RequestPath = "/browser"
            });                    // ..(new DirectoryBrowserOptions) - with this instance we can
                                   //   set our own wished directory to browse on server
                                   // "/browser" - user should request "localhost:5000/browser" to
                                   //   reach this directory browser
            app.UseStaticFiles();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();                // app.UseStaticFiles() - use static files at root
                                                 //   (localhost:5000/) path
            app.UseStaticFiles("/staticfiles");  // app.UseStaticFiles(/staticfiles) - use static
                                                 //   files at localhost:5000/staticfiles path
                                                 //
                                                 //   Now we can use static files on both
                                                 //     / and /staticfiles paths
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider("/tmp"),
                RequestPath = new PathString("/pile")
            });                                  // app.UseStaticFiles(new StaticFileOptions() {..})
                                                 //   - use static files under our wished system
                                                 //   path and under our wished request path
                                                 //
                                                 //   Now we got three request pathes to use
                                                 //     static files

            app.UseFileServer();  // app.UseFileServer() - this method combines all three static
                                  //   files middleware abilities:
                                  //       > static files
                                  //       > default files
                                  //       > directory browsing
                                  //
                                  //   By default it works at localhost:5000/ request path, and
                                  //     ability of default files enabled
                                  //
                                  //   You also can configure an instance of FileServerOptions class

            app.Run((context) => context.Response.WriteAsync("Use static file ability"));
        }
    }
}
