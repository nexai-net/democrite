// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.UI.Services
{
    using Democrite.Toolbox.Abstractions.Proxies;
    using Democrite.Toolbox.WPF.Abstractions.Navigations;
    using Democrite.Toolbox.WPF.Abstractions.Views;
    using Democrite.Toolbox.WPF.Services;

    using Microsoft.Extensions.DependencyInjection;

    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ContentControlViewManager : BaseViewManager
    {
        #region Fields

        private readonly Dictionary<string, Window> _windows;

        private string? _currentViewId;
        private ContentControl? _contentControl;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentControlViewManager"/> class.
        /// </summary>
        public ContentControlViewManager(IEnumerable<ViewRelation> relations,
                                         IServiceProvider serviceProvider,
                                         IDispatcherProxy dispatcherProxy)
            : base(relations, serviceProvider, dispatcherProxy)
        {
            this._windows = new Dictionary<string, Window>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Setups the view content control target.
        /// </summary>
        public void SetupViewContentControlTarget(ContentControl contentControl)
        {
            this._contentControl = contentControl;
        }

        /// <inheritdoc />
        protected override object? Display(Func<object?> viewModelBuilder,
                                           ViewRelation relation,
                                           bool dialog,
                                           NavigationArguments? arguments,
                                           string? specializedId)
        {

            var newId = relation.Uid + (string.IsNullOrEmpty(specializedId) ? string.Empty : specializedId);

            if (this._windows.TryGetValue(newId, out var existingWindow))
            {
                existingWindow.Activate();
                return existingWindow.DataContext;
            }

            if (this._currentViewId == newId)
                return this._contentControl?.DataContext;

            var view = ActivatorUtilities.CreateInstance(this.ServiceProvider, relation.View) as FrameworkElement;

            ArgumentNullException.ThrowIfNull(view, "ViewRelation.View must be a FrameworkElement");

            view.DataContext = viewModelBuilder();

            if (view is Window windowView)
            {
                this._windows.Add(newId, windowView);

                windowView.Closed -= WindowView_Closed;
                windowView.Closed += WindowView_Closed;

                if (dialog)
                    windowView.ShowDialog();
                else
                    windowView.Show();
            }
            else
            {
                ArgumentNullException.ThrowIfNull(this._contentControl);

                var previousContent = this._contentControl.Content;
                this._contentControl.Content = null;

                if (previousContent is IDisposable dispo)
                    dispo.Dispose();

                this._contentControl.Content = view;
                this._currentViewId = newId;
            }

            return view.DataContext;

        }

        /// <summary>
        /// Windows the view closed.
        /// </summary>
        private void WindowView_Closed(object? sender, EventArgs e)
        {
            var kvWindow = this._windows.FirstOrDefault(w => w.Value == sender);
            if (kvWindow.Value is not null)
                this._windows.Remove(kvWindow.Key);
        }

        #endregion
    }
}
