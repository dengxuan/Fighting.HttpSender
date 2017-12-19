using Baibaocp.LotteryDispatcher.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Baibaocp.LotteryDispatcher.Builder
{
    public class LotteryDispatcherBuilder
    {
        private readonly DiscoverySettings _discoverySettings = new DiscoverySettings();

        public IServiceCollection Services { get; }

        internal LotteryDispatcherBuilder(IServiceCollection services)
        {
            Services = services;
        }

        private void AddHandlerDiscovery()
        {
            Services.Scan(s =>
            {
                s.FromApplicationDependencies()
                .AddClasses(f => f.AssignableTo(typeof(IExecuteHandler<>)), !_discoverySettings.IncludeNonPublic)
                .UsingRegistrationStrategy(_discoverySettings.RegistrationStrategy)
                .AsSelf()
                .WithLifetime(_discoverySettings.DiscoveredHandlersLifetime);
            });

            Services.Scan(s =>
            {
                s.FromApplicationDependencies()
                .AddClasses(f => f.AssignableTo(typeof(IExecuterDispatcher<>)), !_discoverySettings.IncludeNonPublic)
                .UsingRegistrationStrategy(_discoverySettings.RegistrationStrategy)
                .AsImplementedInterfaces()
                .WithLifetime(_discoverySettings.DiscoveredHandlersLifetime);
            });
        }

        public LotteryDispatcherBuilder ConfigureOptions(Action<LotteryDispatcherOptions> options)
        {
            Services.Configure(options);
            return this;
        }

        public LotteryDispatcherBuilder AddHandlerDiscovery(Action<DiscoverySettings> discoverySettings)
        {
            discoverySettings(_discoverySettings);
            return this;
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<LotteryDispatcherOptions>, DefaultLotteryDispatcherOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<LotteryDispatcherOptions>>().Value);
            AddHandlerDiscovery();
        }
    }
}
