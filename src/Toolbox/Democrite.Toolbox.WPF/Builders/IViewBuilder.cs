// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Builders
{
    using Democrite.Toolbox.WPF.Abstractions.Views;

    /// <summary>
    /// 
    /// </summary>
    public interface IViewBuilder
    {
        /// <summary>
        /// Associate view and view model
        /// </summary>
        IViewBuilder Register<TView, TViewModel>();

        /// <summary>
        /// Associate view and view model with a specific <paramref name="key"/>
        /// </summary>
        IViewBuilder Register<TView, TViewModel>(string key);

        /// <summary>
        /// Associate view and view model with a specific <paramref name="viewRelationInfo"/>
        /// </summary>
        IViewBuilder Register<TView, TViewModel>(ViewRelationInfo viewRelationInfo);
    }
}
