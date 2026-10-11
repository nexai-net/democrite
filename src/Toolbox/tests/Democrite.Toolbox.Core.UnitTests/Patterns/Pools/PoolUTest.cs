// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Core.UnitTests.Patterns.Pools
{
    using Democrite.Toolbox.Patterns.Pools;

    using NFluent;

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Test for <see cref="Pool<>"/>
    /// </summary>
    public sealed class PoolUTest
    {
        public sealed class TestPoolItem : PoolBaseItem
        {
            /// <inheritdoc />
            protected override void OnCleanUp()
            {
            }
        }

        /// <summary>
        /// Try recycling on not thread safe pool
        /// </summary>
        [Fact]
        public void UnSafe_Pool()
        {
            var pool = new Pool<TestPoolItem>(12);

            var items = new HashSet<TestPoolItem>();

            for (int i = 0; i < 100; i++)
            {
                var it = pool.GetItem();

                items.Add(it);

                if (i > 5)
                    it.Release();
            }

            Check.That(items.Count).IsEqualTo(7);
        }

        /// <summary>
        /// Thread safe pool test
        /// </summary>
        [Fact]
        public async void ThreadSafe_Pool()
        {
            const int TEST_SIZE = 120;

            var pool = new Pool<TestPoolItem>(500);

            var items = new ConcurrentBag<TestPoolItem>();

            var tasks = Enumerable.Range(0, TEST_SIZE)
                                  .Select(i =>
                                  {
                                      return Task.Run(() =>
                                      {
                                          var it = pool.GetItem();

                                          items.Add(it);

                                          if (i > 5)
                                              it.Release();
                                      });
                                  }).ToArray();

            await tasks.SafeWhenAllAsync(default);

            Check.That(items.Distinct().Count()).IsStrictlyLessThan(TEST_SIZE);
        }
    }
}
