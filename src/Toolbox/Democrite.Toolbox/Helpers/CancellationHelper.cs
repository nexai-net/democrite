// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Helpers
{
    using Democrite.Toolbox.Abstractions.Disposables;
    using Democrite.Toolbox.Disposables;

    using System;

    /// <summary>
    /// Help cencallation algorithme
    /// </summary>
    public static class CancellationHelper
    {
        #region Fields

        private static readonly TimeSpan s_defaultScopeTimeout = TimeSpan.FromSeconds(10);

        #endregion

        /// <summary>
        /// Create a timeout <see cref="CancellationToken"/>
        /// </summary>
        public static CancellationToken Timeout(TimeSpan? timeout = null)
        {
            return new CancellationTokenSource(timeout ?? s_defaultScopeTimeout).Token;
        }

        /// <summary>
        /// Create a timeout <see cref="CancellationToken"/>
        /// </summary>
        public static ISafeDisposable<CancellationToken> DisposableTimeout(TimeSpan? timeout = null)
        {
            var source = new CancellationTokenSource(timeout ?? s_defaultScopeTimeout);
            return new DisposableAction<CancellationToken>(_ => source.Dispose(), source.Token);
        }

        /// <summary>
        /// Associate with a using provide a safe scope accesible by only one thread at time
        /// </summary>
        public static ISafeDisposable<CancellationToken> SingleAccessScope(SemaphoreSlim locker,
                                                                           Func<CancellationTokenSource?> getCurrentSource,
                                                                           Action<CancellationTokenSource> setterNewSource,
                                                                           TimeSpan? timeout = null,
                                                                           params CancellationToken[] extraTokens)
        {
#if DEBUG
            ArgumentNullException.ThrowIfNull(locker);
            ArgumentNullException.ThrowIfNull(getCurrentSource);
            ArgumentNullException.ThrowIfNull(setterNewSource);
#endif

            try
            {
                locker.Wait(timeout ?? s_defaultScopeTimeout);

                var current = getCurrentSource();
                current?.Cancel();

                if (extraTokens.Any())
                    current = CancellationTokenSource.CreateLinkedTokenSource(extraTokens);
                else
                    current = new CancellationTokenSource();
                setterNewSource(current);

                return new DisposableAction<CancellationToken>(static _ => { }, current.Token);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                locker.Release();
            }
        }
    }
}
