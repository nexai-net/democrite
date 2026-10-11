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

    /// <summary>
    /// Base class implementation of <see cref="IViewManager"/> that manager the window openning
    /// </summary>
    /// <seealso cref="Democrite.Toolbox.WPF.Services.BaseViewManager" />
    public abstract class BaseViewWithWindowManager : BaseViewManager
    {
        #region Field

        private static readonly Type s_windowType;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes the <see cref="BaseViewWithWindowManager"/> class.
        /// </summary>
        static BaseViewWithWindowManager()
        {
            s_windowType = typeof(Window);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewWithWindowManager"/> class.
        /// </summary>
        protected BaseViewWithWindowManager(IEnumerable<ViewRelation> relations,
                                            IServiceProvider serviceProvider,
                                            IDispatcherProxy dispatcherProxy) 
            : base(relations, serviceProvider, dispatcherProxy)
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected sealed override object? Display(Func<object?> viewModelBuilder,
                                                  ViewRelation relation,
                                                  bool dialog,
                                                  NavigationArguments? arguments,
                                                  string? specializedId)
        {
            if (relation.View.IsAssignableTo(s_windowType))
            {
                var inst = this.ServiceProvider.GetRequiredService(relation.View) as Window;

                ArgumentNullException.ThrowIfNull(inst);

                var windowVM = viewModelBuilder();
                inst.DataContext = windowVM;

                if (dialog)
                    inst.ShowDialog();
                else
                    inst.Show();

                return windowVM;
            }

            return OnDisplay(viewModelBuilder, relation, dialog, arguments, specializedId);
        }

        /// <inheritdoc cref="Display(Func{object?}, ViewRelation, bool, NavigationArguments?, string?)" />
        protected abstract object? OnDisplay(Func<object?> viewModelBuilder, ViewRelation relation, bool dialog, NavigationArguments? arguments, string? specializedId);

        #endregion
    }
}
