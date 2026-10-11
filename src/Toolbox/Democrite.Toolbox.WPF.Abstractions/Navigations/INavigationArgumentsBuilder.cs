// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Abstractions.Navigations
{
    /// <summary>
    /// 
    /// </summary>
    public interface INavigationArgumentsBuilder
    {
        /// <summary>
        /// Adds specified value in the navigation arguments
        /// </summary>
        INavigationArgumentsBuilder Add<TValue>(string key, TValue? value);
    }
}
