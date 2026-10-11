// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Patterns.Graphs.Map
{
    using System.Collections.Generic;

    /// <summary>
    /// Serializable information for the <see cref="DijkstraGraphNavigationMap"/>
    /// </summary>
    public sealed record class DijkstraGraphNavigationMapDefinition(IReadOnlyCollection<string> Nodes, int[][]? Map);
}