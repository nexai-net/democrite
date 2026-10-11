// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.ComServer.Abstraction.Test
{
    using Democrite.Toolbox.Abstractions.Attributes;

    [RefRoute("chat")]
    public sealed record class ChatMessage(string From, string Message, DateTime CreateTime);

    public sealed record class OtherClientsRequest();

    public sealed record class OtherClientsResponse(IReadOnlyCollection<string> Clients);
}
