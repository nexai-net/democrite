// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Framework.Builders.Signals
{
    using Democrite.Framework.Core;
    using Democrite.Framework.Core.Abstractions.Enums;
    using Democrite.Framework.Core.Abstractions.Signals;

    using System;

    /// <inheritdoc cref="ISignalBuilder" />
    internal sealed class SignalBuilder : SignalNetworkBasePartBuilder<ISignalBuilder>, ISignalBuilder
    {
        #region Fields
        
        private SignalDefinition? _parent;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalBuilder"/> class.
        /// </summary>
        public SignalBuilder(string simpleNameIdentifier, string? displayName = null, Guid? uid = null, Action<IDefinitionMetaDataBuilder>? metaDataBuilder = null)
            : base(simpleNameIdentifier, displayName, uid, metaDataBuilder)
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public ISignalBuilder Parent(SignalDefinition parent)
        {
            this._parent = parent;
            return this;
        }

        /// <inheritdoc />
        public SignalDefinition Build()
        {
            //TO-DO from Opus 5.5 Parent signal ignored when building definition : Parent(...) stores _parent, but Build() never passes it to SignalDefinition (whose ctor accepts parentSignalId). Every sub-signal made with CreateSubSignal/StartBuildSubSignal therefore loses its parent link. Pass `this._parent?.SignalId` as the parentSignalId argument.
            return new SignalDefinition(this.Uid,
                                        RefIdHelper.Generate(RefTypeEnum.Signal, this.SimpleNameIdentifier, this.DefinitionMetaData?.NamespaceIdentifier),
                                        this.DisplayName ?? this.SimpleNameIdentifier,
                                        this.DefinitionMetaData);
        }

        #endregion
    }
}
