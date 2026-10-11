// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.UI.Exceptions
{
    using Democrite.Toolbox.WPF.UI.Resources;

    using System;
    using System.Windows.Controls;

    /// <summary>
    /// Raised when some needed part in the template are missing
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class TemplatePartMissingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatePartMissingException"/> class.
        /// </summary>
        public TemplatePartMissingException(Type partType, string partName, Control control)
            : base(ExceptionSR.TemplatePartMissingException.WithArguments(partName, partType, control))
        {

        }
    }
}
