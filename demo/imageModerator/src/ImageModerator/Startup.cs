using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
//using WebApiWithSignalR.Controllers;

namespace SignalRImageModerator
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ImageModeratorHub>();

            //services.AddSignalR();   //Without Redis Cache


            services.AddSignalR().AddRedis(options => options.Factory = writer =>
            {
                return ConnectionMultiplexer.Connect("localhost", writer);
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

	       app.UseFileServer();
 
            app.UseSignalR(routes =>
            {
               routes.MapHub<ImageModeratorHub>("moderate");
            });
            app.UseMvc();

        }
    }
}
