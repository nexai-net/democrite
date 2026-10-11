// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.Serializations
{
    using Newtonsoft.Json;

    /// <summary>
    /// Extend <see cref="JsonSerializerSettings"/>
    /// </summary>
    public static class JsonSerializerSettingsExtensions
    {
        /// <summary>
        /// Use fluent api to configure <see cref="JsonSerializerSettings"/>
        /// </summary>
        public static JsonSerializerSettings Configure(this JsonSerializerSettings settings, Action<IJsonSerializerSettingConfigBuilder> builder)
        {
            ArgumentNullException.ThrowIfNull(settings);
            ArgumentNullException.ThrowIfNull(builder);

            var configBuilder = new JsonSerializerSettingConfigBuilder();
            builder(configBuilder);
            configBuilder.Build(settings);

            return settings;
        }
    }
}
