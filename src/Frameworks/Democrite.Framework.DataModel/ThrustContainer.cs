// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Framework.DataModel
{
    using Orleans;

    using System.ComponentModel;

    /// <summary>
    /// Data container with level of thrust
    /// </summary>
    [Immutable]
    [Serializable]
    [GenerateSerializer]
    [ImmutableObject(true)]
    //TO-DO from Opus 5.5 Typo in public type name Thrust : The type is named ThrustContainer while it carries a TrustLevel; the misspelling is baked into the public and serialized API. Rename to TrustContainer (keeping an [Alias]/obsolete shim for compatibility).
    public sealed record class ThrustContainer<TData>(TData Data, double TrustLevel);
}