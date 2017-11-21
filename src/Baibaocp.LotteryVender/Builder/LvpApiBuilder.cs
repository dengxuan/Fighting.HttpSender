using Baibaocp.LotteryVender.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Baibaocp.LotteryVender.Builder
{
    public class LvpApiBuilder
    {
        private readonly DiscoverySettings _discoverySettings = new DiscoverySettings();

        public IServiceCollection Services { get; }

        internal LvpApiBuilder(IServiceCollection services)
        {
            Services = services;
        }

        private void AddHandlerDiscovery()
        {
            Services.Scan(s =>
                        s.FromApplicationDependencies()
                    .AddClasses(f => f.AssignableTo(typeof(IExecuteHandler<>)), !_discoverySettings.IncludeNonPublic)
                    .UsingRegistrationStrategy(_discoverySettings.RegistrationStrategy)
                    .AsImplementedInterfaces()
                    .WithLifetime(_discoverySettings.DiscoveredHandlersLifetime));
        }

        public LvpApiBuilder ConfigureOptions(Action<ExecuterOptions> options)
        {
            Services.Configure(options);
            return this;
        }

        public LvpApiBuilder AddHandlerDiscovery(Action<DiscoverySettings> discoverySettings)
        {
            discoverySettings(_discoverySettings);
            return this;
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ExecuterOptions>, DefaultLvpApiOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<ExecuterOptions>>().Value);
            AddHandlerDiscovery();
        }
    }
}
