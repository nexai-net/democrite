// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.UI.Selectors
{
    using Democrite.Toolbox.WPF.UI.Helpers;

    using System.Windows;

    /// <summary>
    /// Use a property and a binding to match
    /// </summary>
    public abstract class PropertyBaseConditonalSelector<TItem> : ConditonalBaseItem<TItem>, IConditionalSelector<TItem>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public System.Windows.Data.Binding? Binding { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object? Value { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override bool ValidateItemUsage(DependencyObject dependencyObject, object? dataContext)
        {
            if (this.Binding is null)
                return false;

            object? compareValue = null;

            if (dataContext is not null)
                compareValue = BindingHelper.GetBindingResult(this.Binding, dataContext);
            
            if (dependencyObject is not null && compareValue is null)
                compareValue = BindingHelper.GetBindingResult(this.Binding, dependencyObject);

            return this.Value?.Equals(compareValue) ?? compareValue is null;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PropertyConditonalTemplateSelector : PropertyBaseConditonalSelector<DataTemplate>, IConditionalDataTemplate
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PropertyConditonalStyleSelector : PropertyBaseConditonalSelector<Style>, IConditionalStyle
    {
    }
}
