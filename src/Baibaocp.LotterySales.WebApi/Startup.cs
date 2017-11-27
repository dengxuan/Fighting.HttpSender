using Baibaocp.Core.Messages;
using Baibaocp.LotteryVender.Messaging;
using Baibaocp.Storaging.EntityFrameworkCore;
using Fighting.AspNetCore.WebApi.DependencyInjection;
using Fighting.Extensions.Caching.DependencyInjection;
using Fighting.Extensions.Caching.Redis.DependencyInjection;
using Fighting.Extensions.Messaging.DependencyInjection;
using Fighting.Extensions.Serialization.Json.DependencyInjection;
using Fighting.Extensions.Serialization.ProtoBuf.DependencyInjection;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace Baibaocp.LotteryVender.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFightingWebApi();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<ValidateModelAttribute>();
            });
            services.AddMvcCore().AddAuthorization();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = "http://localhost:5001";
                        options.ApiName = "LotteryVender.WebApi";
                        options.RequireHttpsMetadata = false;
                    });

            services.AddJson();

            services.AddMessaging(builderAction =>
            {
                builderAction.AddRbbitPublisher(rabbitOptions =>
                {
                    rabbitOptions.VirtualHost = "/";
                    rabbitOptions.Servername = "192.168.1.21";
                    rabbitOptions.Username = "root";
                    rabbitOptions.Password = "123qwe";
                    rabbitOptions.ExchangeName = "Baibaocp.LotteryVender";
                })
                .ConfigureOptions(messageOpgions =>
                {
                    //messageOpgions.AddProducer<OrderingMessage>("Lvp.Orders");
                });
            });

            services.AddCaching(builderAction =>
            {
                builderAction.AddRedis(setupAction =>
                {
                    setupAction.DatabaseId = 10;
                    setupAction.ConnectionString = "192.168.1.21:6379,password=zf8Mjjo6rLKDzf81";
                });
            });

            services.AddEntityFrmaeworkStorage<BaibaocpStorage>(storageOptions =>
           {
               storageOptions.DefaultNameOrConnectionString = "server=192.168.1.21; database=Baibaocp; uid=dba; password=L4H]JtuA2RaWl@^]S$9a4dN-!,01Z7Qs;";
           });

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new Info { Title = "Lottery Vender API", Version = "v1" });
                setupAction.DocInclusionPredicate((docName, description) => true);
                setupAction.IncludeXmlComments(string.Format("{0}/Baibaocp.LotteryVender.WebApi.xml", AppDomain.CurrentDomain.BaseDirectory));

                // Define the BearerAuth scheme that's in use
                setupAction.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                // Assign scope requirements to operations based on AuthorizeAttribute
                //setupAction.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            //app.UseStaticFiles();

            app.UseMvc();

            app.UseSwagger(setupAction =>
            {
                setupAction.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.ConfigureOAuth2("100010", "secret", "", "Baibaocp.LotteryVender");
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "Lottery Vender API V1");
            });
        }
    }
}
