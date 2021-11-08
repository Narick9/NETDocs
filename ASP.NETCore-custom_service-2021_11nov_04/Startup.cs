using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCore_custom_service_2021_11nov_04
{
    public class Startup_
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public interface IMessageSender
        {
            string Send();
        }
        public class EmailMessageSender : IMessageSender
        {
            public string Send() => "Sent by Email";
        }
        public class SmsMessageSender : IMessageSender
        {
            public string Send() => "Sent by SMS";
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMessageSender, EmailMessageSender>();
                        // services.AddTransient<...>() - addes an transient type service to
                        //   that service collection
                        //
                        //   Here we used the overload of this method, that sets a bind between
                        //     IServiceType and IImplementation (they are ...<T, Y>())
                        //
                        //   There are a lot of built-in services in ASP.NET Core. All they have
                        //     special extension method to parameter services
                        //
                        //         services.UseMvc(), for example
                        //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
                              IMessageSender messageSender)
        {                               // Configure(..., ImessageSender ...) - now we can receive
                                        //   an instance of our service and use it here, in
                                        //   the pipeline
                                        //
                                        //   Since we binded IMessageSender as IServiceType,
                                        //     we should get exactly IMessageSender parameter,
                                        //     despite that we actually will receive
                                        //     EmailMessageSender object
                                        //
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(context =>
            {
                return context.Response.WriteAsync(messageSender.Send());
                                    // Then, if we wish, we can just in ConfigureService()
                                    //   from above, in services.AddTransient()
                                    //   replace EmailMessageSender type with SmsMessageSender, and
                                    //   used its method Send()
            });
        }
    }
    public class Startup
    {
        public interface ICounter
        {
            int Value { get; }
        }
        public class RandomCounter : ICounter
        {
            public int Value { get; }
            public RandomCounter()
            {
                Value = (new Random()).Next(1, 1000);
            }
        }
//      public class CounterService
//      {
//          protected internal ICounter Counter { get; }
//          public CounterService(ICounter counter)
//          {
//              Counter = counter;
//          }
//      }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<ICounter, RandomCounter>();
            //services.AddTransient<RandomCounter>();
            //services.AddScoped<ICounter, RandomCounter>();
            //services.AddScoped<RandomCounter>();  // AddScoped<RandomCounter>() - adds new service
                                                    //   . But since we add it here, explecitly in
                                                    //   ConfigureServices(), RandomCounter counts
                                                    //   as distinct service. Then each HTTPS
                                                    //   request will receive two different objects
                                                    //   of type RandomCounter
            //services.AddSingleton<ICounter, RandomCounter>();
            //services.AddSingleton<RandomCounter>();
            services.AddScoped<ICounter, RandomCounter>();
            services.AddScoped<RandomCounter>(provider =>
            {
                return (RandomCounter)provider.GetService(typeof(ICounter));
            });
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<CounterMiddleware>();
        }
        public class CounterMiddleware
        {
            RequestDelegate _next;
            int id = 0;
            public CounterMiddleware(RequestDelegate next)
            {
                _next = next;
            }
            public Task Invoke(HttpContext context, ICounter count1, RandomCounter count2,
                               RandomCounter count3)
            {
                id++;
                context.Response.ContentType = "text/html;charset=utf-8";
                context.Response.WriteAsync(
                    $"Id: {id}, count1: {count1.Value}, count2: {count2.Value}, " +
                    $"count3: {count3.Value}");
                return Task.CompletedTask;
            }
        }
    }
}
