using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.AspNetCore.Routing;

namespace ASP.NETCore_routing_2021_11nov_12
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();  // app.UseRouting() - sets RoutingMiddleware that just can
                               //   refer requested paths to matched methods
                               //
                               //       Requested path + HTTP thing ("/index HTTP: GET") is Endpoint
                               //       Endpoint + RoutePattern is RouteEndpoint
                               //
                               //****but you could do the same with app.Map() and app.MapWhen() but
                               //    without app.UseRouting()
                               //
                               //   Here we just defined that we will use Endpoints. They will
                               //     be added later (at app.UseEndpoints())
                               //
            app.Use((context, next) =>
            {
                               // app.Use() - here, between app.UseRouting() and app.UseEndpoints(),
                               //   we can make a logic layer (in most cases it's just a filter)
                Endpoint endpoint = context.GetEndpoint();
                if (endpoint != null)
                {
                    context.Response.WriteAsync($"endpoint: _{endpoint}_\n");
                                                        // {endpoint} - (with request
                                                        //   http://localhost:5000/wet) is string
                                                        //   "/wet HTTP: GET"
                    
                    var routePattern = (endpoint as Microsoft.AspNetCore.Routing.RouteEndpoint)
                        .RoutePattern;
                    Microsoft.AspNetCore.Routing.RouteEndpoint routeEndpoint =
                        endpoint as Microsoft.AspNetCore.Routing.RouteEndpoint;

                    context.Response.WriteAsync($"routeEndpoint: _{routeEndpoint}_\n");
                                                        // {routeEndpoint} - (with request
                                                        //   http://localhost:5000/wet) is string
                                                        //   "/wet HTTP: GET"
                    context.Response.WriteAsync($"routeEndpoint.DisplayName: " +
                        $"_{routeEndpoint.DisplayName}_\n");
                                                        // {...DisplayName} - (with request
                                                        //   http://localhost:5000/wet) is string
                                                        //   "/wet HTTP: GET"
                    context.Response.WriteAsync($"routeEndpoint.RoutePattern.RawText: " +
                        $"_{routeEndpoint.RoutePattern.RawText}_\n");
                                                        // {...RawText} - (with request
                                                        //   http://localhost:5000/wet) is string
                                                        //   "/wet"
                    return next.Invoke();
                }
                else
                {
                                    // else - if routing machine can't match given request with
                                    //   endpoint (that is, there is no MapGet() for such path),
                                    //   our endpoint variable will contain null
                    return context.Response.WriteAsync("endpoint is null!\n");
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGet("/wet", async context =>
                {
                    await context.Response.WriteAsync("Hello wet!");
                });
            });
        }
        public void Configure(IApplicationBuilder app)
        {
                                // There is also second way
            RouteHandler handler = new RouteHandler(Handler);
            RouteBuilder builder = new RouteBuilder(app, handler);
            builder.MapRoute("default", "{category}/{thing}");
                            // builder.MapRoute() - adds a route to builder
                            //
                            //      "default" - route name
                            //      "{category}/{thing}" - URL pattern for rout.
                            //                             Our route will be matched to every
                            //                             localhost:5000/blabla/foofoo request
                            //
            builder.MapRoute("{category}/{section}/{thing}", context =>
            {
                            // builder.MapRoute("...", context => {...}) - this method much is
                            //   much more easier way
                return context.Response.WriteAsync("You gave me three segments");
            });
            builder.MapRoute("store/{action}", contex =>
            {
                            // "store/{action}" - this route will be matched only to these requests,
                            //   that have string "store" as first segment.
                            //
                            //       localhost:5000/store/fugu, for example
                            //
                            //   Such routes called "static routes"
                            //
                            //   Actually user will never reach this route, since route with
                            //     {section}/{thing} patter catches every two-segments requests
                            //
                return contex.Response.WriteAsync("YOu gave me two segemtns. " +
                    "First of them is store!");
            });
            app.UseRouter(builder.Build());
                            // builder.Build() - gives completed router
                            // app.UseRouter() - adds RouterMiddleware
                            //
                            //   If request doesn't match to storaged routes, then handling
                            //     will continue. app.Run() below will complete it

            app.Run(context =>
            {
                return context.Response.WriteAsync("Hello from app.Run()!");
            });
        }
        public Task Handler(HttpContext context)
        {
            return context.Response.WriteAsync("Hello from two-segments handler!");
        }
    }
}
