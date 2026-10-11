// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Core.UnitTests.Extensions
{
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Test for <see cref="ReflectionImplExtensions"/>
    /// </summary>
    public sealed class ReflectionExtensionsUTests
    {
        private static readonly MethodInfo s_genericMethod;

        static ReflectionExtensionsUTests()
        {
            Expression<Func<int, string, int>> func = (i, s) => Method(i, s);

            s_genericMethod = ((MethodCallExpression)func.Body).Method.GetGenericMethodDefinition();
        }

        //TO-DO from Opus 5.5 Test without any assertion : Method_Caching_System computes two hash codes and asserts nothing, and it does not call any Democrite code, so it can never fail. Either assert the expected caching behavior of the ReflectionExtensions under test or delete the test.
        [Fact]
        public void Method_Caching_System()
        {
            var keys = new[] { typeof(double), typeof(string) };

            var method = s_genericMethod.MakeGenericMethod(keys);
            var method2 = s_genericMethod.MakeGenericMethod(keys);

            var refCode = RuntimeHelpers.GetHashCode(method);
            var refCode2 = RuntimeHelpers.GetHashCode(method2);
        }

        private static TValue Method<TValue, Tvalue2>(TValue value, Tvalue2 str)
        {
            return value;
        }
    }
}
