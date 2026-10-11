// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Abstractions.Supports
{
    using System.Threading.Tasks;

    /// <summary>
    /// Support update action
    /// </summary>
    public interface ISupportUpdate
    {
        /// <summary>
        /// Updates the specified target with source.
        /// </summary>
        void Update(object? source);
    }

    /// <summary>
    /// Support update action
    /// </summary>
    public interface ISupportUpdateAsync : ISupportUpdate
    {
        /// <summary>
        /// Updates the specified target with source.
        /// </summary>
        ValueTask UpdateAsync(object? source);
    }

    /// <summary>
    /// Support update action
    /// </summary>
    public interface ISupportUpdateAsync<TSource> : ISupportUpdateAsync
    {
        /// <summary>
        /// Updates the specified target with source.
        /// </summary>
        ValueTask UpdateAsync(TSource? source);
    }

    /// <summary>
    /// Support update action
    /// </summary>
    public interface ISupportUpdate<TSource> : ISupportUpdate
    {
        /// <summary>
        /// Updates the specified target with source.
        /// </summary>
        void Update(TSource? source);
    }
}
