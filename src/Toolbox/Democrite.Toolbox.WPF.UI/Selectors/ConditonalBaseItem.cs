// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.UI.Selectors
{
    using System.Windows;

    /// <summary>
    /// 
    /// </summary>
    public abstract class ConditonalBaseItem<TItem> : DependencyObject, IConditionalSelector<TItem>
    {
        #region Fields

        //TO-DO from Opus 5.5 Item DP hard-typed as DataTemplate : The generic base registers ItemProperty with typeof(DataTemplate) under the name "TItem", so setting a Style on the Style selectors (PropertyConditonalStyleSelector, TypeConditonalStyleSelector) throws because the value type is invalid. Register it with typeof(TItem) and nameof(Item).
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(TItem),
                                                                                             typeof(DataTemplate),
                                                                                             typeof(ConditonalBaseItem<TItem>),
                                                                                             new PropertyMetadata(default(TItem)));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        public TItem Item
        {
            get { return (TItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool TryGetItem(DependencyObject dependencyObject, object? dataContext, out TItem? conditionalItem)
        {
            conditionalItem = this.Item;

            return ValidateItemUsage(dependencyObject, dataContext);
        }

        /// <summary>
        /// Validates item usage condition
        /// </summary>
        protected abstract bool ValidateItemUsage(DependencyObject dependencyObject, object? dataContext);

        #endregion
    }
}
