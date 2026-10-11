// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF
{
    using Democrite.Toolbox.Abstractions.Disposables;
    using Democrite.Toolbox.Abstractions.Proxies;
    using Democrite.Toolbox.Disposables;

    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Disposable <see cref="BaseViewModel"/>
    /// </summary>
    /// <seealso cref=".BaseViewModel" />
    /// <seealso cref="ISafeDisposable" />
    public abstract class AsyncDisposableBaseViewModel : BaseViewModel, ISafeAsyncDisposable
    {
        #region Fields

        private long _disposableCount;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDisposableBaseViewModel"/> class.
        /// </summary>
        /// <param name="dispatcherProxy"></param>
        protected AsyncDisposableBaseViewModel(IDispatcherProxy dispatcherProxy)
            : base(dispatcherProxy)
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AsyncDisposableBaseViewModel"/> class.
        /// </summary>
        ~AsyncDisposableBaseViewModel()
        {
            Task.Run(() => DisposeAsync(true)).Wait();
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public bool IsDisposed
        {
            get { return Interlocked.Read(ref this._disposableCount) > 0; }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return DisposeAsync(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        private async ValueTask DisposeAsync(bool fromFinalizer)
        {
            if (Interlocked.Increment(ref this._disposableCount) > 1)
                return;

            await DisposeBeginAsync();

            await DisposeAllResourcesAsync();

            if (!fromFinalizer)
                await DisposeManagedAsync();

            await DisposeUnmanagedAsync();

            await DisposeEndAsync();
        }

        /// <inheritdoc cref="SafeDisposable.DisposeBegin"/>
        protected virtual ValueTask DisposeBeginAsync()
        {
            return ValueTask.CompletedTask;
        }

        /// <inheritdoc cref="SafeDisposable.DisposeUnmanaged"/>
        protected virtual ValueTask DisposeUnmanagedAsync()
        {
            return ValueTask.CompletedTask;
        }

        /// <inheritdoc cref="SafeDisposable.DisposeManaged"/>
        protected virtual ValueTask DisposeManagedAsync()
        {
            return ValueTask.CompletedTask;
        }

        /// <inheritdoc cref="SafeDisposable.DisposeEnd"/>
        protected virtual ValueTask DisposeEndAsync()
        {
            return ValueTask.CompletedTask;
        }

        #endregion
    }
}
