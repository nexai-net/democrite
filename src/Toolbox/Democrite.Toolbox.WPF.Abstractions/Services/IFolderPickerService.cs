// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Abstractions.Services
{
    public interface IFolderPickerService
    {
        /// <summary>
        /// Picks the folder path asynchronous.
        /// </summary>
        ValueTask<string?> PickFolderPathAsync(string? root = null);
    }
}
