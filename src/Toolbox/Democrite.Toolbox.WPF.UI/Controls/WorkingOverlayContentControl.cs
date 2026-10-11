// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.UI.Controls
{
    using Democrite.Toolbox.WPF.UI.Helpers;

    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Simple <see cref="ContentControl"/> that managed <see cref="WaitingOverlay"/> by default
    /// </summary>
    /// <seealso cref="ContentControl" />
    public sealed class WorkingOverlayContentControl : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingOverlayContentControl"/> class.
        /// </summary>
        public WorkingOverlayContentControl()
        {
            SetValue(WaitingOverlayHelper.EnabledProperty, true);
            SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            SetValue(ContentControl.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch);
            SetValue(ContentControl.VerticalContentAlignmentProperty, VerticalAlignment.Stretch);
        }
    }
}
