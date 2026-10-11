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
    public abstract class DisposableBaseViewModel : BaseViewModel, ISafeDisposable
    {
        #region Fields

        private long _disposableCount;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableBaseViewModel"/> class.
        /// </summary>
        /// <param name="dispatcherProxy"></param>
        protected DisposableBaseViewModel(IDispatcherProxy dispatcherProxy)
            : base(dispatcherProxy)
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableBaseViewModel"/> class.
        /// </summary>
        ~DisposableBaseViewModel()
        {
            //TO-DO from Opus 5.5 Finalizer disposes managed resources : The finalizer path still runs DisposeBegin, DisposeAllResourcesAsync (which disposes other managed objects and may call DispatcherProxy.Throw) and DisposeEnd, and blocks with .Wait(). This breaks the Dispose pattern. In the finalizer path, call only DisposeUnmanaged, or drop the finalizer since the type owns no unmanaged handles.
            Dispose(true);
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
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        private void Dispose(bool fromFinalizer)
        {
            if (Interlocked.Increment(ref this._disposableCount) > 1)
                return;

            DisposeBegin();

            //TO-DO from Opus 5.5 Sync-over-async Dispose can deadlock UI thread : Dispose is typically called on the dispatcher thread (WindowExt.OnClosed, ViewClosed), and it blocks on async resource disposal. Any resource that marshals back to the UI thread (SendAndWait/SendAsync) deadlocks, and exceptions come back wrapped in AggregateException. Make resource disposal synchronous here, or have callers use the async-disposable base.
            Task.Run(() => DisposeAllResourcesAsync()).Wait();

            if (!fromFinalizer)
                DisposeManaged();

            DisposeUnmanaged();

            DisposeEnd();
        }

        /// <inheritdoc cref="SafeDisposable.DisposeBegin"/>
        protected virtual void DisposeBegin()
        {
        }

        /// <inheritdoc cref="SafeDisposable.DisposeUnmanaged"/>
        protected virtual void DisposeUnmanaged()
        {
        }

        /// <inheritdoc cref="SafeDisposable.DisposeManaged"/>
        protected virtual void DisposeManaged()
        {
        }

        /// <inheritdoc cref="SafeDisposable.DisposeEnd"/>
        protected virtual void DisposeEnd()
        {
        }

        #endregion
    }
}
