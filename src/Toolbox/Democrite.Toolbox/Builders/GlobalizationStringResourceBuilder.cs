// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Builders
{
    using Democrite.Toolbox.Abstractions.Services;
    using Democrite.Toolbox.Services;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class GlobalizationStringResourceBuilder : IGlobalizationStringResourceBuilder
    {
        #region Fields
        
        private readonly IServiceCollection _serviceDescriptors;
        
        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalizationStringResourceBuilder"/> class.
        /// </summary>
        public GlobalizationStringResourceBuilder(IServiceCollection serviceDescriptors)
        {
            this._serviceDescriptors = serviceDescriptors;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add RESXs the string resource.
        /// </summary>
        public IGlobalizationStringResourceBuilder RESXStringResource<TResource>()
        {
            this._serviceDescriptors.TryAddEnumerable(ServiceDescriptor.Singleton<IGlobalizationStringResourceSourceProvider, GlobalizationStringResourceFromRESXSourceProvider<TResource>>());
            return this;
        }

        #endregion
    }
}
