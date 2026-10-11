// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Serializations
{
    using System;

    /// <summary>
    /// Define a service able validate if the convertions could apply following the algorithme setups
    /// </summary>
    internal interface IDemocriteJsonObjectConverterConditions
    {
        /// <summary>
        /// Validates the conditions
        /// </summary>
        bool Validate(Func<string, Tuple<bool, object?>> getJsonValue, Type objectType, object? existingValue);
    }
}
