// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Benchmark.Sandbox
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;

    using Democrite.Toolbox.Abstractions.Conditions;

    using System;
    using System.Linq.Expressions;

    [MemoryDiagnoser(true)]
    [SimpleJob(RunStrategy.Throughput, iterationCount: 100, invocationCount: 10_000)]
    public class ConditionalExpressionBenchmark
    {
        private static readonly ConditionExpressionDefinition s_conditionalExpression;

        static ConditionalExpressionBenchmark()
        {
            Expression<Func<string, bool>> func = (string str) => str != null && str.Length > 0 && str.StartsWith("42");
            s_conditionalExpression = func.Serialize();
        }

        [Benchmark]
        public LambdaExpression ConditionToExpression()
        {
            return s_conditionalExpression.ToExpression<string, bool>(useCache: false);
        }

        [Benchmark]
        public LambdaExpression ConditionToExpressionWithCache()
        {
            return s_conditionalExpression.ToExpression<string, bool>(useCache: true);
        }
    }
}
