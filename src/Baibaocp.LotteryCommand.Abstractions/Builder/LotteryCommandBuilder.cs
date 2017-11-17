using Baibaocp.LotteryCommand.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Baibaocp.LotteryCommand.Builder
{
    public class LotteryCommandBuilder
    {
        private readonly DiscoverySettings _discoverySettings = new DiscoverySettings();

        public IServiceCollection Services { get; }

        internal LotteryCommandBuilder(IServiceCollection services)
        {
            Services = services;
        }

        private void AddHandlerDiscovery()
        {
            Services.Scan(s =>
                        s.FromAssemblies(_discoverySettings.CommandHandlerAssemblies)
                    .AddClasses(f => f.AssignableTo(typeof(ICommandHandlerAsync<>)), !_discoverySettings.IncludeNonPublic)
                    .UsingRegistrationStrategy(_discoverySettings.RegistrationStrategy)
                    .AsImplementedInterfaces()
                    .WithLifetime(_discoverySettings.DiscoveredHandlersLifetime));

            foreach (var assembly in _discoverySettings.CommandHandlerAssemblies)
            {
                IEnumerable<Type> types = assembly.GetTypes().Where(predicate =>
                {
                    return predicate.Name == "Setup";
                });
                foreach (var item in types)
                {
                    object @object = Activator.CreateInstance(item);
                    MethodInfo method = item.GetMethod("Init");
                    method.Invoke(@object, new object[] { Services });
                }
            }

            Services.TryAddSingleton(_discoverySettings.CommandHandlerAssemblies);
        }

        public LotteryCommandBuilder ConfigureOptions(Action<CommandOptions> options)
        {
            Services.Configure(options);
            return this;
        }

        public LotteryCommandBuilder AddHandlerDiscovery(Action<DiscoverySettings> discoverySettings)
        {
            discoverySettings(_discoverySettings);
            return this;
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<CommandOptions>, DefaultCommandOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<CommandOptions>>().Value);
            AddHandlerDiscovery();
        }
    }
}
