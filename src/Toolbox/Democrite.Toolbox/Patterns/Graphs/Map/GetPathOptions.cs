// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Patterns.Graphs.Map
{
    using System.Collections.Generic;

    /// <summary>
    /// Define option to build the node path
    /// </summary>
    public record struct GetPathOptions(int MaxToleratePathSize = -1,
                                        IReadOnlyCollection<string>? FilterRelationTypes = null,
                                        IReadOnlyCollection<string>? ExcludeRelationTypes = null);
}