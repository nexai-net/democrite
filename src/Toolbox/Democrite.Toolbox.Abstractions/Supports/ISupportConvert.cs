// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Abstractions.Supports
{
    /// <summary>
    /// Support type conversion to some types
    /// </summary>
    public interface ISupportConvert
    {
        /// <summary>
        /// Try convert current instance to <typeparamref name="TTarget"/>
        /// </summary>
        bool TryConvert<TTarget>(out TTarget? target);

        /// <summary>
        /// Try convert current instance to <typeparamref name="TTarget"/>
        /// </summary>
        bool TryConvert(out object? target, Type targetType);
    }

    /// <summary>
    /// Support type conversion to <typeparamref name="TTarget"/>
    /// </summary>
    public interface ISupportConvert<TTarget>
    {
        /// <summary>
        /// Converts this instance to <typeparamref name="TTarget"/>
        /// </summary>
        TTarget? Convert();
    }
}
