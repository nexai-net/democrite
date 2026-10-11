// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Abstractions.Proxies
{
    /// <summary>
    /// Tag the class as a proxy service for a specific one
    /// </summary>
    /// <remarks>
    ///     Usefull when DPI injection requeried generic and factory a proxy is needed
    ///     With this class you can acces the final one without passing through the proxy
    /// </remarks>
    public interface IProxyService<TService>
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        TService GetService();
    }
}
