// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISelectorService<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the value based on configured data
        /// </summary>
        TValue? GetValue(in TKey key);
    }
}
