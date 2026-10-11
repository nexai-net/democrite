// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Disposables
{
    public sealed class DisposableContainer<TContent> : SafeDisposable<TContent>
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableContainer{TContent}"/> class.
        /// </summary>
        public DisposableContainer(TContent content, bool disposeContent = false) 
            : base(content, disposeContent)
        {
        }

        #endregion
    }
}
