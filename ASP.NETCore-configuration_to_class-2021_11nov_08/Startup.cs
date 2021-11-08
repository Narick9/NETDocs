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

namespace ASP.NETCore_configuration_to_class_2021_11nov_08
{
    public class Startup
    {
        IConfiguration config;
        public Startup(IConfiguration _config)
        {
            config = new ConfigurationBuilder().AddConfiguration(_config)
                .AddJsonFile("./person.json").Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            Person thomas = new Person();
            config.Bind(thomas);
                            // Also you can use config.Get<Person>() method that return new and
                            //   already binded instance
                            //
                            // You can do the same and with .xml (by its syntax)

            app.Run((context) =>
            {
                string message = $"thomas.Name: {thomas.Name}\n" +
                    $"thomas.Age: {thomas.Age}\n" +
                    "thomas.Languages:\n";
                foreach (string curr in thomas.Languages)
                {
                    message += $"\t{curr}\n";
                }
                message += $"thomas.Company.Title: {thomas.Company.Title}\n";
                message += $"thomas.Company.Country: {thomas.Company.Country}\n";
                return context.Response.WriteAsync(message);
            });
        }
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public List<string> Languages { get; set; }
            public Company Company { get; set; }
        }
        public class Company
        {
            public string Title { get; set; }
            public string Country { get; set; }
        }
    }
}
