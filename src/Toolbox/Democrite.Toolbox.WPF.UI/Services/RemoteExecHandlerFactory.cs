// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.UI.Services
{
    using Democrite.Toolbox.Abstractions.Services;
    using Democrite.Toolbox.Disposables;
    using Democrite.Toolbox.WPF.Abstractions.Services;

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Factory in charge to produce <see cref="IRemoteExecHandler"/>
    /// </summary>
    /// <seealso cref="IRemoteExecHandlerFactory" />
    public sealed class RemoteExecHandlerFactory : SafeDisposable, IRemoteExecHandlerFactory
    {
        #region Fields

        private readonly Dictionary<Uri, IRemoteExecHandler> _remoteExecHandlers;
        private readonly ReaderWriterLockSlim _locker;

        private readonly IFileSystemHandler _fileSystemHandler;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteExecHandlerFactory"/> class.
        /// </summary>
        public RemoteExecHandlerFactory(IFileSystemHandler fileSystemHandler)
        {
            this._remoteExecHandlers = new Dictionary<Uri, IRemoteExecHandler>();
            this._locker = new ReaderWriterLockSlim();

            this._fileSystemHandler = fileSystemHandler;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRemoteExecHandler GetRemoteExecHandler(Uri uri, string toolName, string executableName)
        {
            this._locker.EnterReadLock();
            try
            {
                if (this._remoteExecHandlers.TryGetValue(uri, out var executor))
                    return executor;
            }
            finally
            {
                this._locker.ExitReadLock();
            }

            this._locker.EnterWriteLock();
            try
            {
                if (this._remoteExecHandlers.TryGetValue(uri, out var executor))
                    return executor;

                var exe = new RemoteExecHandler(uri, toolName, executableName, this._fileSystemHandler);
                this._remoteExecHandlers.Add(uri, exe);
                return exe;
            }
            finally
            {
                this._locker.ExitWriteLock();
            }
        }

        #endregion
    }
}
