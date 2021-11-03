using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_web_2021_10oct_20
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure1(IApplicationBuilder app, IWebHostEnvironment env)
        {
//          Стандартный код по шаблону выглядит так:
//
//          if (env.IsDevelopment())
//          {
//              app.UseDeveloperExceptionPage();
//          }
//
//          app.UseRouting();
//
//
//          app.UseEndpoints(endpoints =>
//          {
//              endpoints.MapGet("/", async context =>
//              {
//                  await context.Response.WriteAsync("Hello World!");
//              });
//          });
//
//
//          int count = 0;
//          app.Run(async context =>
//          {
//              await context.Response.WriteAsync($"count: {count}");
//          });

            int x = 15;
            int y = 4;
            int z = 0;
            app.Use(async (httpContext, next) =>
            {
                z = x + y;
                await next.Invoke();  // next - делегат, ссылающийся на следующий компонент
                                      //   middleware'а (на делегат, отправленный ниже в app.Run())
            });
            app.Run(async httpContext =>
            {
                await httpContext.Response.WriteAsync($"z: {z}");
            });


            //  app.Use(async (httpContext, next) =>
            //  {
            //      await httpContext.Response.WriteAsync("Hello, World!");
            //      await next.Invoke();    // httpContext...WriteAsync() - как говорит Metanit,
            //                              //   компонент, встраиваемый через метод app.Use(),
            //                              //   должен избегать в себе вызова
            //                              //   httpContext...WriteAsync(), т.к. последующие
            //                              //   вызовы этого метода (в следующих команентах) могут
            //                              //   нарушить цепочку отправки
            //                              //
            //                              //   Правда в самой документации .NET Docs такого я не
            //                              //     нашёл, а статья метанита 2019-го года
            //                              //
            //                              //   Лечился этот недуг выставлением задержки между
            //                              //     двумя разными вызовами httpContext...WriteAsync()
            //  });
            //  app.Run(async (httpContext) =>
            //  {
            //      await httpContext.WriteLine("Goodbye...");
            //  });


        }
        public void Configure2(IApplicationBuilder app)
        {
            app.UseRouting();

            int x = 2;
            app.Use(async (httpContext, next) =>
            {
                x *= 2;  // x --- 2 * 2 = 4
                await next.Invoke();  // await next.Invoke() - жизнь компонентов middleware
                                      //   разделяется на до вызова этого метода и после
                                      //   (об этом говорилось ещё в моих записях)
                x *= 2;  // x --- 8 * 2 = 16
                await httpContext.Response.WriteAsync($"Result: {x}");
            });
            app.Run(async (httpContext) =>
            {
                x *= 2;  // x --- 4 * 2 = 8
                await Task.FromResult(0);
            });
        }
    }
}
