// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Abstractions.Services
{
    using Democrite.Toolbox.WPF.Abstractions.Models;

    /// <summary>
    /// Handler installation and remote communication
    /// </summary>
    public interface IRemoteExecHandler
    {
        /// <summary>
        /// Executes  target external after installed
        /// </summary>
        Task<RemoteExecResultRemoteExecResult> ExecuteAsync(CancellationToken token, params string[] arguments);
    }
}
