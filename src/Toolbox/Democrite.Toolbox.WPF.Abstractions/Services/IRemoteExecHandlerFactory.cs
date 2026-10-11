// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Abstractions.Services
{
    using System;

    /// <summary>
    /// Factory used to create <see cref="IRemoteExecHandler"/>
    /// </summary>
    public interface IRemoteExecHandlerFactory
    {
        /// <summary>
        /// Gets the remote execute handler from an external provider
        /// </summary>
        IRemoteExecHandler GetRemoteExecHandler(Uri uri, string toolName, string executableName);
    }
}
