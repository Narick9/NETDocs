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
using Microsoft.Extensions.Options;

namespace ASP.NETCore_ioptions_2021_11nov_08
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
            services.Configure<Person>(config);  // sevices.Configure<Person>() - it is an
                                                 //   equivalent to
                                                 //       services.AddSingleton<IOptions<Person>>()
                                                 //   (****i think it's singleton, but maybe I
                                                 //   wrong)
            services.Configure<Company>(config.GetSection("Company"));
                                                 // services.Configure<Company>() - actually it
                                                 //   will not be used here
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseMiddleware<PersonMiddleware>();
        }
        class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public List<string> Languages { get; set; }
            public Company Company { get; set; }
        }
        class Company
        {
            public string Title { get; set; }
            public string Country { get; set; }
        }
        class PersonMiddleware
        {
            readonly RequestDelegate next;
            public Person person;
            public PersonMiddleware(RequestDelegate _next, IOptions<Person> options)
            {
                next = _next;
                person = options.Value;
            }
            public Task Invoke(HttpContext context)
            {
                string message = $"person.Name: {person.Name}\n" +
                    $"person.Age: {person.Age}\n" +
                    "person.Languages:\n";
                foreach (string curr in person.Languages)
                {
                    message += $"\t{curr}\n";
                }
                message += $"person.Company.Title: {person.Company.Title}\n";
                message += $"person.Company.Country: {person.Company.Country}\n";
                return context.Response.WriteAsync(message);
            }
        }
    }
}
