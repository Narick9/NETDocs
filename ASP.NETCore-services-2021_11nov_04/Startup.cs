using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace ASP.NETCore_services_2021_11nov_04
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private IServiceCollection _services;
        public void ConfigureServices(IServiceCollection services)
        {                       // servieces - already contains 78 dependency injections (aka
                                //   services)
                                //
                                //       For example, IWebHostEnvironment is a service
                                //
            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run((context) =>
            {
                StringBuilder message = new StringBuilder();
                message.Append("<h1>Default services</h1>");
                message.Append("<table>");
                message.Append("<tr><th>Type</th><th>Lifetime</th><th>Implementation</th></tr>");
                foreach (ServiceDescriptor curr in _services)
                {
                    message.Append("<tr>");
                    message.Append($"<td>{curr.ServiceType.FullName}</td>");
                    message.Append($"<td>{curr.Lifetime}</td>");
                    message.Append($"<td>{curr.ImplementationType?.FullName}</td>");
                    message.Append("</tr>");
                }
                message.Append("</table>");
                context.Response.ContentType = "text/html;charset=utf-8";
                return context.Response.WriteAsync(message.ToString());
            });
        }
    }
}
