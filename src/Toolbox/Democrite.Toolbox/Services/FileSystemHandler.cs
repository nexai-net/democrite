// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Services
{
    using Democrite.Toolbox.Abstractions.Services;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    /// <inheritdoc cref="IFileSystemHandler"/>
    public sealed class FileSystemHandler : IFileSystemHandler
    {
        #region Ctor

        /// <summary>
        /// Initializes the <see cref="FileSystemHandler"/> class.
        /// </summary>
        static FileSystemHandler()
        {
            Instance = new FileSystemHandler();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static FileSystemHandler Instance { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public Stream OpenRead(Uri uri)
        {
            return File.OpenRead(uri.LocalPath);
        }

        /// <inheritdoc />
        public async ValueTask<bool> WriteToFileAsync(Stream stream, Uri uri, bool @override = true, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(uri);

            var preparationSucceed = CheckAndPrepareFileTarget(uri, @override);
            if (preparationSucceed)
            {
                //TO-DO from Opus 5.5 Copy loop allocates whole file, may never end : Math.Max allocates a buffer the size of the whole file instead of capping it at 64 KB. The loop also compares bytes read against stream.Length, so a stream whose position is not 0, or a read that returns 0 early, makes it spin forever. Use Math.Min, or simply await stream.CopyToAsync(outputStream, token).
                var buffer = new byte[Math.Max(stream.Length, 65536)];

                using (var outputStream = File.OpenWrite(uri.LocalPath))
                {
                    var readTotal = 0;
                    var remains = stream.Length;

                    while (remains > 0)
                    {
                        var readData = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        await outputStream.WriteAsync(buffer, 0, readData, token);

                        readTotal += readData;
                        remains = stream.Length - readTotal;
                    }
                }
            }

            return preparationSucceed;
        }

        /// <inheritdoc />
        public async ValueTask<bool> WriteToFileAsync(byte[] bytes, Uri uri, bool @override = true, CancellationToken token = default)
        {
            ArgumentNullException.ThrowIfNull(bytes);
            ArgumentNullException.ThrowIfNull(uri);

            var preparationSucceed = CheckAndPrepareFileTarget(uri, @override);
            if (preparationSucceed)
                await File.WriteAllBytesAsync(uri.LocalPath, bytes, token);

            return preparationSucceed;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<Uri> SearchFiles(in string root, in string searchPattern, bool recursive)
        {
            var rootUri = new Uri(root, UriKind.Absolute);
            return Directory.GetFiles(root, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                            .Select(s => new Uri(rootUri, s))
                            .ToArray();
        }

        /// <inheritdoc />
        public bool Exists(Uri uri)
        {
            return uri.IsFile && (File.Exists(uri.LocalPath) || Directory.Exists(uri.LocalPath));
        }

        /// <inheritdoc />

        /// <remarks>
        ///     // Add method to set working dir
        /// </remarks>
        public Uri MakeUriAbsolute(Uri uri)
        {
            if (uri.IsAbsoluteUri)
                return uri;

            return MakeUriAbsolute(uri.OriginalString);
        }

        /// <remarks>
        ///     // Add method to set working dir
        /// </remarks>
        public Uri MakeUriAbsolute(in string uri)
        {
            var currentDir = Directory.GetCurrentDirectory();

            if (!currentDir.EndsWith("/"))
                currentDir += "/";

            return new Uri(new Uri(currentDir), uri);
        }

        /// <inheritdoc />
        public bool IsFile(Uri target)
        {
            var attr = File.GetAttributes(target.LocalPath);

            return attr.HasFlag(FileAttributes.Directory) == false;
        }

        /// <inheritdoc />
        public ValueTask<string> GetTemporaryFolderAsync(bool global, CancellationToken token)
        {
            if (global)
            {
                var globalTempPath = Path.Combine(Path.GetTempPath(), nameof(Democrite));
                return ValueTask.FromResult(globalTempPath);
            }

            var localTempFolder = MakeUriAbsolute(".tmp");
            //TO-DO from Opus 5.5 Temp folder returned as URI, not path : MakeUriAbsolute builds a file:// URI, so OriginalString returns 'file:///C:/.../.tmp' rather than a filesystem path, unlike the global branch, which returns a plain path. Return localTempFolder.LocalPath.
            return ValueTask.FromResult(localTempFolder.OriginalString);
        }

        /// <inheritdoc />
        public ValueTask<bool> Delete(Uri file)
        {
            var exists = Exists(file);
            if (exists)
                //TO-DO from Opus 5.5 Delete uses URI string instead of local path : For absolute file:// URIs, OriginalString is 'file:///...', which File.Delete rejects. Exists() also returns true for directories, so File.Delete fails on those. Use file.LocalPath and handle directories separately.
                File.Delete(file.OriginalString);
            return ValueTask.FromResult(exists);
        }

        /// <summary>
        /// Deletes the specified folder
        /// </summary>
        public ValueTask<bool> DeleteFolder(Uri folder, bool recurive = true)
        {
            if (Directory.Exists(folder.LocalPath))
            {
                Directory.Delete(folder.LocalPath, recurive);
                return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
        }

        /// <inheritdoc />
        public async ValueTask<bool> CopyFromAsync(Uri source, Uri target, bool @override = true, CancellationToken token = default)
        {
            if (!Exists(source))
                return false;

            using (var sourceStream = OpenRead(source))
            {
                return await CopyFromAsync(sourceStream, target, @override, token);
            }
        }

        /// <inheritdoc />
        public async ValueTask<bool> CopyFromAsync(Stream source, Uri target, bool @override = true, CancellationToken token = default)
        {
            var targetIsPrepared = CheckAndPrepareFileTarget(target, @override);

            if (targetIsPrepared)
                return await WriteToFileAsync(source, target, @override, token);

            return false;
        }

        #region Tools

        /// <summary>
        /// Checks the and prepare file target.
        /// </summary>
        private bool CheckAndPrepareFileTarget(Uri uri, bool @override)
        {
            if (@override == false && Exists(uri))
                return false;

            if (Exists(uri))
                Delete(uri);

            var filePath = uri.LocalPath;
            var dir = Path.GetDirectoryName(filePath)!;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return true;
        }

        #endregion

        #endregion
    }
}
