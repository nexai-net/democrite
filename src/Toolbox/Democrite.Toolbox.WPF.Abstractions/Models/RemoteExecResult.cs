// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Abstractions.Models
{
    /// <summary>
    /// 
    /// </summary>
    public record class RemoteExecResultRemoteExecResult(bool Success, string StandardLog, string ErrorLog, int ExitCode, string ExceptionMessage);
}
