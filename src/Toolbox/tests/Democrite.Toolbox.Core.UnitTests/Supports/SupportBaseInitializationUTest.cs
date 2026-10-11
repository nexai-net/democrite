// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.UnitTests.Supports
{
    using Democrite.Toolbox.Supports;

    using NFluent;

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Unit test about <see cref="SupportBaseInitialization"/>
    /// </summary>
    public sealed class SupportBaseInitializationUTest
    {
        #region Nested

        private sealed class TestSupportInitialization : SupportBaseInitialization<int>
        {
            #region Fields

            private readonly TaskCompletionSource _completionSource;
            private long _initializationCounter;

            #endregion

            #region Ctor

            /// <summary>
            /// Initializes a new instance of the <see cref="TestSupportInitialization"/> class.
            /// </summary>
            public TestSupportInitialization(TaskCompletionSource completionSource)
            {
                this._completionSource = completionSource;
            }

            #endregion

            #region Properties

            public long InitializationCounter
            {
                get { return Interlocked.Read(ref this._initializationCounter); }
            }

            #endregion

            #region Methods

            /// <inheritdoc />
            protected override async ValueTask OnInitializingAsync(int initializationState, CancellationToken token)
            {
                await this._completionSource.Task;

                Interlocked.Increment(ref this._initializationCounter);
            }

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ensure <see cref="SupportInitializationImplementation"/> allow only one initialization
        /// </summary>
        [Fact]
        public async Task SupportInitializationImplementation_EnsureOnlyOneCall()
        {
            var initCounter = 0;
            var completionSource = new TaskCompletionSource();

            var inst = new SupportInitializationImplementation<int>((_, token) =>
            {
                Interlocked.Increment(ref initCounter);
                return new ValueTask(completionSource.Task);
            });

            Check.That(initCounter).IsEqualTo(0);

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsFalse();

            var task = inst.InitializationAsync(42);

            // Wait to be sure the completion task is waiting
            //TO-DO from Opus 5.5 Fixed sleep to reach initializing state : Both EnsureOnlyOneCall tests wait 1 s and assume initialization has started. This is slow and can still fail on a loaded CI agent. Have the init delegate signal a TaskCompletionSource when it is entered and await that before asserting IsInitializing (the same applies at line 134).
            await Task.Delay(1000);

            Check.That(inst.IsInitializing).IsTrue();
            Check.That(inst.IsInitialized).IsFalse();

            completionSource.TrySetResult();

            await task;

            Check.That(initCounter).IsEqualTo(1);

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsTrue();

            Check.That(initCounter).IsEqualTo(1);

            await inst.InitializationAsync(42);

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsTrue();

            Check.That(initCounter).IsEqualTo(1);
        }

        /// <summary>
        /// Ensure <see cref="SupportBaseInitialization"/> allow only one initialization
        /// </summary>
        [Fact]
        public async Task SupportBaseInitialization_EnsureOnlyOneCall()
        {
            var completionSource = new TaskCompletionSource();

            var inst = new TestSupportInitialization(completionSource);

            Check.That(inst.InitializationCounter).IsEqualTo(0);

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsFalse();

            var task = inst.InitializationAsync(42);

            // Wait to be sure the completion task is waiting
            await Task.Delay(1000);

            Check.That(inst.IsInitializing).IsTrue();
            Check.That(inst.IsInitialized).IsFalse();

            completionSource.TrySetResult();

            await task;

            Check.That(inst.InitializationCounter).IsEqualTo(1);

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsTrue();

            Check.That(inst.InitializationCounter).IsEqualTo(1);

            await inst.InitializationAsync(42);

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsTrue();

            Check.That(inst.InitializationCounter).IsEqualTo(1);
        }

        /// <summary>
        /// Ensure <see cref="SupportInitializationImplementation"/> allow only one initialization even if theire many parallel call
        /// </summary>
        [Fact]
        public async Task SupportInitializationImplementation_EnsureOnlyOneCall_ManyParallelCall()
        {
            var initCounter = 0;
            var completionSource = new TaskCompletionSource();

            var inst = new SupportInitializationImplementation<int>((_, token) =>
            {
                Interlocked.Increment(ref initCounter);
                return new ValueTask(completionSource.Task);
            });

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsFalse();

            var tasks = Enumerable.Range(0, 42)
                                  .Select(_ => Task.Run(async () => await inst.InitializationAsync(42)))
                                  .ToArray();

            completionSource.TrySetResult();

            await Task.WhenAll(tasks);

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsTrue();

            Check.That(initCounter).IsEqualTo(1);

        }

        /// <summary>
        /// Ensure <see cref="SupportBaseInitialization"/> allow only one initialization even if theire many parallel call
        /// </summary>
        [Fact]
        public async Task SupportBaseInitialization_EnsureOnlyOneCall_ManyParallelCall()
        {
            var completionSource = new TaskCompletionSource();

            var inst = new TestSupportInitialization(completionSource);

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsFalse();

            var tasks = Enumerable.Range(0, 42)
                                  .Select(_ => Task.Run(async () => await inst.InitializationAsync(42)))
                                  .ToArray();

            completionSource.TrySetResult();

            await Task.WhenAll(tasks);

            Check.That(inst.IsInitializing).IsFalse();
            Check.That(inst.IsInitialized).IsTrue();

            Check.That(inst.InitializationCounter).IsEqualTo(1);

        }

        #endregion
    }
}
