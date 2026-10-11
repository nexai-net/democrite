// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Abstractions.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// File picker
    /// </summary>
    public interface IFilePickerService
    {
        /// <summary>
        /// Picks the existing file path.
        /// </summary>
        ValueTask<string?> PickExistingFileAsync(params string[] filters);

        /// <summary>
        /// Picks the existing file path.
        /// </summary>
        ValueTask<IReadOnlyCollection<string>?> PickExistingFilesAsync(params string[] filters);
    }
}
