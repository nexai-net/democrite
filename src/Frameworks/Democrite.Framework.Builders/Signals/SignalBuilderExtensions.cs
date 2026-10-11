// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

// Keep : Democrite.Framework.Builders
namespace Democrite.Framework.Builders
{
    using Democrite.Framework.Builders.Signals;
    using Democrite.Framework.Core.Abstractions.Signals;

    using System;

    /// <summary>
    /// Extension about signal building
    /// </summary>
    public static class SignalBuilderExtensions
    {
        /// <summary>
        /// Starts the building a sub signal.
        /// </summary>
        public static ISignalBuilder StartBuildSubSignal(this SignalDefinition parentSignal, string simpleNameIdentifier, string? displayName = null, Guid? uid = null, bool contactToParentName = true)
        {
            ArgumentNullException.ThrowIfNull(parentSignal);
            ArgumentNullException.ThrowIfNullOrEmpty(simpleNameIdentifier);

            //TO-DO from Opus 5.5 Null displayName yields 'parent:' name : With the default displayName = null and contactToParentName = true, the display name becomes 'parentName:' with an empty suffix (same at line 41). Use `displayName ?? simpleNameIdentifier` before concatenating. Also rename the misspelled 'contactToParentName' parameter to 'concatToParentName'.
            return Signal.StartBuilding(simpleNameIdentifier, contactToParentName ? parentSignal.Name + ":" + displayName : displayName, uid)
                         .Parent(parentSignal);
        }

        /// <summary>
        /// Starts the building a sub signal.
        /// </summary>
        /// <returns>
        ///     Return sub definition created
        /// </returns>
        public static SignalDefinition CreateSubSignal(this SignalDefinition parentSignal, string simpleNameIdentifier, string? displayName = null, Guid? uid = null, bool contactToParentName = true)
        {
            ArgumentNullException.ThrowIfNull(parentSignal);
            ArgumentNullException.ThrowIfNullOrEmpty(simpleNameIdentifier);

            return Signal.StartBuilding(simpleNameIdentifier, contactToParentName ? parentSignal.Name + ":" + displayName : displayName, uid)
                         .Parent(parentSignal)
                         .Build();
        }

        /// <summary>
        /// Starts the building a sub signal.
        /// </summary>
        /// <returns>
        ///     Root definition
        /// </returns>
        public static SignalDefinition CreateSubSignal(this SignalDefinition parentSignal, string simpleNameIdentifier, string? displayName, out SignalDefinition subSignalDefinition, Guid? uid = null, bool contactToParentName = true)
        {
            subSignalDefinition = CreateSubSignal(parentSignal, simpleNameIdentifier, displayName, uid, contactToParentName);
            return parentSignal;
        }
    }
}
