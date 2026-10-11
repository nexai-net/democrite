// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Abstractions.Supports
{
    /// <summary>
    /// Support a public name that could be directly displayed to user
    /// </summary>
    public interface ISupportDisplayName
    {
        /// <summary>
        /// Gets the name of the display.
        /// </summary>
        string DisplayName { get; }
    }
}
