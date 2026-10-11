// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Disposables
{
    public sealed class AsyncDisposableContainer<TContent> : SafeAsyncDisposable<TContent>
    {
        #region Fields
        
        private readonly bool _disposeContent;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDisposableContainer{TContent}"/> class.
        /// </summary>
        public AsyncDisposableContainer(TContent content, bool disposeContent = false) 
            : base(content)
        {
            this._disposeContent = disposeContent;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override async ValueTask DisposeBeginAsync()
        {
            if (this._disposeContent && this.Content is IAsyncDisposable disposable)
                await disposable.DisposeAsync();
            await base.DisposeBeginAsync();
        }

        #endregion
    }
}
