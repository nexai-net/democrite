// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Services
{
    using Democrite.Toolbox.Abstractions.Services;
    using Democrite.Toolbox.Extensions;

    using System.IO;
    using System.Reflection;

    /// <inheritdoc cref="IAssemblyLoader"/>
    /// <seealso cref="IAssemblyLoader" />
    public sealed class AssemblyLoader : IAssemblyLoader
    {
        #region Methods

        /// <inheritdoc />
        public Assembly Load(string path)
        {
            return Assembly.LoadFile(path);
        }

        /// <inheritdoc />
        public Assembly Load(Stream stream)
        {
            using (stream)
            {
                return Assembly.Load(stream.ReadAll());
            }
        }

        /// <inheritdoc />
        public Assembly Load(AssemblyName assemblyName)
        {
            return Assembly.Load(assemblyName);
        }

        #endregion
    }
}
