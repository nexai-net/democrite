// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Serializations
{
    /// <summary>
    /// Root builder of <see cref="JsonSerializerSettingsExtensions"/>
    /// </summary>
    public interface IJsonSerializerSettingConfigBuilder
    {
        /// <summary>
        /// Gets the converter.
        /// </summary>
        IJsonSerializerConverterBuilder Converter { get; }
    }

    /// <summary>
    /// Builder deficated to converters
    /// </summary>
    public interface IJsonSerializerConverterBuilder
    {
        /// <summary>
        /// Add a converter to solve concret type from a possible interface or abstract type
        /// </summary>
        IJsonSerializerTypeResolverBuilder<TType> For<TType>();

        /// <summary>
        /// Manually add <see cref="IDemocriteJsonObjectConverter"/>.
        /// </summary>
        IJsonSerializerConverterBuilder Add(IDemocriteJsonObjectConverter? jsonObjectConverter);

        /// <summary>
        /// End the current build session a turn back to root for a new type mapping
        /// </summary>
        IJsonSerializerSettingConfigBuilder DoneConverter { get; }
    }

    /// <summary>
    /// Internal object to build all the converters
    /// </summary>
    internal interface IJsonSerializerSettingConfigBuilderInternal
    {
        void Build();
    }
}
