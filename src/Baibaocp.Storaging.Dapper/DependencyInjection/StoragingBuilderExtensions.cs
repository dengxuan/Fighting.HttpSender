using Fighting.Storaging;
using Fighting.Storaging.Builder;
using Fighting.Storaging.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.Data.MySql;
using System;
using System.Data;
using System.Reflection;

namespace Baibaocp.Storaging.Dapper.DependencyInjection
{
    public static class StoragingBuilderExtensions
    {
        public static StorageBuilder AddDapper(this StorageBuilder builder, Action<StorageOptions> options)
        {
            builder.ConfigureOptions(options);
            builder.Services.AddTransient<IDbConnection>(sp =>
            {
                StorageOptions storageOptions = sp.GetRequiredService<StorageOptions>();
                return new MySqlConnection(storageOptions.DefaultNameOrConnectionString);
            });
            builder.Services.Scan(s =>
            {
                s.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(f => f.AssignableTo(typeof(IRepository<,>)))
                .AsImplementedInterfaces()
                .WithLifetime(ServiceLifetime.Transient);
            });
            return builder;
        }
    }
}
