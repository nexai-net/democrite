// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Builders
{
    /// <summary>
    /// Builder used to setup Globalization resources
    /// </summary>
    public interface IGlobalizationStringResourceBuilder
    {
        /// <summary>
        /// Add RESXs the string resource.
        /// </summary>
        IGlobalizationStringResourceBuilder RESXStringResource<TResource>();
    }
}
