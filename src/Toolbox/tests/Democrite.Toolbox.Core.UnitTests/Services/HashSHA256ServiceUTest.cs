// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Core.UnitTests.Services
{
    using Democrite.Toolbox.Services;

    using System.Security.Cryptography;

    public class HashSHA256ServiceUTest : HashBaseServiceUTest<HashSHA256Service>
    {
        public HashSHA256ServiceUTest() 
            : base(SHA256.Create)
        {
        }
    }
}
