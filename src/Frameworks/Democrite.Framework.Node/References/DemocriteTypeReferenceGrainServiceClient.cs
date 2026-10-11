// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Framework.Node.References
{
    using Democrite.Framework.Core.Abstractions.Models;

    using Orleans.Runtime;
    using Orleans.Runtime.Services;
    using Orleans.Services;

    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Service in charge to solve the <see cref="Uri"/> RefId
    /// </summary>
    internal sealed class DemocriteTypeReferenceGrainServiceClient : GrainServiceClient<IDemocriteTypeReferenceGrainService>, IGrainServiceClient<IDemocriteTypeReferenceGrainService>, IDemocriteTypeReferenceGrainServiceClient
    {
        #region Fields
        
        private readonly IDemocriteTypeReferenceGrainService _currentGrainService;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="DemocriteTypeReferenceGrainServiceClient"/> class.
        /// </summary>
        public DemocriteTypeReferenceGrainServiceClient(IServiceProvider serviceProvider,
                                                        Silo silo)
            : base(serviceProvider)
        {
            this._currentGrainService = base.GetGrainService(silo.SiloAddress);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public ValueTask<ReferenceTargetRegistry?> GetLatestRegistryAsync(string myCurrentEtag, GrainCancellationToken token)
        {
            return this._currentGrainService.GetLatestRegistryAsync(myCurrentEtag, token);
        }

        /// <inheritdoc />
        public bool IsToUpdate(string currentRegistryEtag)
        {
            var task = this._currentGrainService.IsToUpdate(currentRegistryEtag);
            //TO-DO from Opus 5.5 Blocking wait on grain service call : IsToUpdate makes a synchronous call into a grain service with GetResult(). When it is called from a grain or system-target context it can deadlock, and it always blocks a thread. Make it return ValueTask<bool> and await it in the callers.
            return task.AsTask().GetAwaiter().GetResult();
        }

        #endregion
    }
}
