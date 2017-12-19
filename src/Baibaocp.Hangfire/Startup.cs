using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using StackExchange.Redis;

namespace Baibaocp.Hangfire
{
    public class Startup
    {
        public static ConnectionMultiplexer Redis;

        public Startup(IHostingEnvironment env)
        {
            // Other codes / configurations are omitted for brevity.
            Redis = ConnectionMultiplexer.Connect("192.168.1.21:6379,password=zf8Mjjo6rLKDzf81,defaultDatabase=9");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration =>
            {
                configuration.UseRedisStorage("192.168.1.21:6379,password=zf8Mjjo6rLKDzf81,defaultDatabase=9");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new AdministratorHangfireDashboardAuthorizationFilter() }
            });
        }
    }
}
