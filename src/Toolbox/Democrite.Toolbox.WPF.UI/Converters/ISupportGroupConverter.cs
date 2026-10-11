// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.UI.Converters
{
    using System;
    using System.Globalization;

    /// <summary>
    /// 
    /// </summary>
    public interface ISupportGroupConverter
    {
        /// <inheritdoc cref="IValueConverter.Convert"/>
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
