// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Benchmark.Sandbox
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;

    using Democrite.Toolbox.Abstractions.Expressions;

    using System;
    using System.Linq.Expressions;

    [MemoryDiagnoser(true)]
    [SimpleJob(RunStrategy.Throughput, iterationCount: 100, invocationCount: 10_000)]
    public class MemberInitializationBenchmark
    {
        private static readonly MemberInitializationDefinition s_serializeMemberInit;

        private record struct MemberBenchmarkStruct(string str, int indx, Guid uid);

        static MemberInitializationBenchmark()
        {
            Expression<Func<Guid, MemberBenchmarkStruct>> initMember = (id) => new MemberBenchmarkStruct()
            {
                str = "test",
                indx = 42,
                uid = id
            };

            s_serializeMemberInit = initMember.SerializeMemberInitialization();
        }

        [Benchmark]
        public LambdaExpression MemberInitToExpression()
        {
            return s_serializeMemberInit.ToMemberInitializationExpression(useCache: false);
        }

        [Benchmark]
        public LambdaExpression MemberInitToExpressionWithCache()
        {
            return s_serializeMemberInit.ToMemberInitializationExpression(useCache: true);
        }
    }
}
