// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Framework.Cluster.UnitTests.Services
{
    using Democrite.Framework.Cluster.Services;

    using NFluent;

    using System.Net;

    /// <summary>
    /// Test <see cref="NetworkInspector"/>
    /// </summary>
    public sealed class NetworkInspectorUnitTest
    {
        #region Methods

        /// <summary>
        /// Ensure ip string is not resolved
        /// </summary>
        [Theory]
        [InlineData("135.256.32.1", "135.256.32.1", false)]
        [InlineData("135.145.32.1:4256", "135.145.32.1:4256", true)]
        //TO-DO from Opus 5.5 Test depends on machine DNS resolution : The test expects 'localhost' to resolve to exactly one address, 127.0.0.1, but hosts with IPv6 enabled may return ::1 or several addresses, so it is environment-dependent. Inject a resolver abstraction, or assert that the result contains a loopback address.
        [InlineData("localhost:4256", "127.0.0.1:4256", false)]
        public void SolveHostAddresse_Resolve(string hostName, string? expected, bool ipEndpointParsed)
        {
            var inspector = new NetworkInspector();

            var results = inspector.SolveHostAddresse(hostName);

            if (expected != null)
            {
                Check.That(results).IsNotNull().And.CountIs(1).And.ContainsExactly(expected);

                if (ipEndpointParsed)
                {
                    var parseSuccess = IPEndPoint.TryParse(results.First(), out var parseIp);
                    Check.That(parseSuccess).IsTrue();
                    Check.That(parseIp).IsNotNull();
                    Check.That(parseIp!.Address + ":" + parseIp.Port).IsEqualTo(expected);
                }
            }
            else
            {
                Check.That(results).IsNotNull().And.IsEmpty();
            }
        }

        #endregion
    }
}
