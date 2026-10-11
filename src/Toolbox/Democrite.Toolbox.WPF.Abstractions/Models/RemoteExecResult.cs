// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Abstractions.Models
{
    /// <summary>
    /// 
    /// </summary>
    //TO-DO from Opus 5.5 Public type name duplicated by typo : The public record is named RemoteExecResultRemoteExecResult, which leaks into IRemoteExecHandler's API. Rename it to RemoteExecResult (to match the file name) while the API is still young. Related dead code: in RemoteExecHandler the INFO/ERROR labels are swapped and the event queue is never read.
    public record class RemoteExecResultRemoteExecResult(bool Success, string StandardLog, string ErrorLog, int ExitCode, string ExceptionMessage);
}
