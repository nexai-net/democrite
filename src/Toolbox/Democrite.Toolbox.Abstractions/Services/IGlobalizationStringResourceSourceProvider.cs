// Copyright (c) Nexai.
// This file is licenses to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Abstractions.Services
{
    using System.Globalization;

    /// <summary>
    /// Provider used to distribute application resource through layers
    /// </summary>
    public interface IGlobalizationStringResourceSourceProvider
    {
        /// <summary>
        /// Gets the resource.
        /// </summary>
        bool TryGetResource(string name, CultureInfo cultureInfo, out string value);
    }
}
