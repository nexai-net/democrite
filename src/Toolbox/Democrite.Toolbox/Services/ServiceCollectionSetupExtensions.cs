// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

// KEEP : Microsoft.Extensions.DependencyInjection
namespace Microsoft.Extensions.DependencyInjection
{
    using Democrite.Toolbox.Abstractions.Services;
    using Democrite.Toolbox.Builders;
    using Democrite.Toolbox.Services;

    using Microsoft.Extensions.DependencyInjection.Extensions;

    using System;

    public static class ServiceCollectionSetupExtensions
    {
        /// <summary>
        /// Setups the globalization resources.
        /// </summary>
        public static IServiceCollection SetupGlobalizationResources(this IServiceCollection services, Action<IGlobalizationStringResourceBuilder> resourceBuilder)
        {
            services.TryAddSingleton<IGlobalizationStringResourceProvider, GlobalizationStringResourceProvider>();

            var inst = new GlobalizationStringResourceBuilder(services);
            resourceBuilder(inst);
            return services;
        }
    }
}
