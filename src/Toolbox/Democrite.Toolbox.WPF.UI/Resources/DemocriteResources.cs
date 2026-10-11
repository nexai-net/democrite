// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.UI.Resources
{
    using System.Windows;

    public static class DemocriteResources
    {
        #region Ctor        

        /// <summary>
        /// Initializes the <see cref="DemocriteResources"/> class.
        /// </summary>
        static DemocriteResources()
        {
            Global = new Uri("pack://application:,,,/Democrite.Toolbox.WPF.UI;component/Resources/Global.xaml");
            Converter = new Uri("pack://application:,,,/Democrite.Toolbox.WPF.UI;component/Resources/Converters.xaml");
            DarkTheme = new Uri("pack://application:,,,/Democrite.Toolbox.WPF.UI;component/Resources/Themes/Dark/DarkThemeResources.xaml");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the global.
        /// </summary>
        public static Uri Global { get; }

        /// <summary>
        /// Gets the converter.
        /// </summary>
        public static Uri Converter { get; }

        /// <summary>
        /// Gets the dark theme.
        /// </summary>
        public static Uri DarkTheme { get; }

        #endregion
    }
}
