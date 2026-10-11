// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Patterns.Messenger
{
    using Democrite.Toolbox.Abstractions.Supports;

    /// <summary>
    /// A channel is specialized in a type of message and can be extra specialized in a specific category
    /// It store the subscription and distribute the incomming message
    /// </summary>
    internal interface IMessageQueueChannel : ISupportDebugDisplayName
    {
    }
}
