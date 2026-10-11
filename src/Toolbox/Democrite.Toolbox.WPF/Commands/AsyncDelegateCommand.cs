// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Commands
{
    using Democrite.Toolbox.Abstractions.Commands;
    using Democrite.Toolbox.Abstractions.Proxies;

    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Async delegate command
    /// </summary>
    /// <seealso cref="IAsyncCommandExt" />
    public class AsyncDelegateCommand : DelegateCommand, IAsyncCommandExt
    {
        #region Fields

        private readonly Func<ValueTask> _callback;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand"/> class.
        /// </summary>
        public AsyncDelegateCommand(IDispatcherProxy dispatcherProxy,
                                    Func<ValueTask> callback,
                                    Predicate<object?>? predicate = null)
            : base(() => throw new NotSupportedException(), predicate)
        {
            ArgumentNullException.ThrowIfNull(dispatcherProxy);
            ArgumentNullException.ThrowIfNull(callback);

            this.DispatcherProxy = dispatcherProxy;
            this._callback = callback;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dispatcher proxy.
        /// </summary>
        public IDispatcherProxy DispatcherProxy { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Execute(object? parameter)
        {
            Task.Run(async () =>
            {
                StartRunningScope();
                try
                {
                    await OnSafeExecuteAsync(parameter);
                }
                catch (Exception ex)
                {
                    this.DispatcherProxy.Throw(ex);
                }
                finally
                {
                    StopRunningScope();
                }
            }).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public ValueTask ExecuteAsync()
        {
            return ExecuteImplAsync(null);
        }

        #region Tools

        /// <inheritdoc />
        protected async ValueTask ExecuteImplAsync(object? parameter)
        {
            StartRunningScope();
            try
            {
                await OnSafeExecuteAsync(parameter);
            }
            finally
            {
                StopRunningScope();
            }
        }

        /// <summary>
        /// Called when [safe execute asynchronous].
        /// </summary>
        protected virtual async ValueTask OnSafeExecuteAsync(object? parameter)
        {
            await this._callback();
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Async delegate command
    /// </summary>
    /// <seealso cref="IAsyncCommandExt" />
    public class AsyncDelegateCommand<TState> : AsyncDelegateCommand, IAsyncCommandExt<TState>
    {
        #region Fields

        private readonly Func<TState?, ValueTask> _stateCallback;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDelegateCommand{TState}"/> class.
        /// </summary>
        public AsyncDelegateCommand(IDispatcherProxy dispatcherProxy,
                                    Func<TState?, ValueTask> callback,
                                    Predicate<object?>? predicate = null)
            : base(dispatcherProxy, () => throw new NotSupportedException(), predicate)
        {
            this._stateCallback = callback;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public ValueTask ExecuteAsync(TState state)
        {
            return ExecuteImplAsync(state);
        }

        /// <inheritdoc />
        protected override async ValueTask OnSafeExecuteAsync(object? parameter)
        {
            await this._stateCallback((TState?)parameter);
        }

        #endregion
    }
}
