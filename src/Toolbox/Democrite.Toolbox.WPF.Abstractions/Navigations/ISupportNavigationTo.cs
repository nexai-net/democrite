// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Abstractions.Navigations
{
    using System.Threading.Tasks;

    /// <summary>
    /// View model that need to be informed when navigate to view occured
    /// </summary>
    public interface ISupportNavigationTo
    {
        /// <summary>
        /// Called when view is navigate to
        /// </summary>
        ValueTask OnNavigateToAsync(NavigationArguments? arguments);
    }
}
