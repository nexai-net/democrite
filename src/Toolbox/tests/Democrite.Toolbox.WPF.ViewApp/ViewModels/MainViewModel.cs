// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.ViewApp.ViewModels
{
    using Democrite.Toolbox.Abstractions.Proxies;

    public sealed class MainViewModel : BaseViewModel
    {
        public MainViewModel(IDispatcherProxy dispatcherProxy) : base(dispatcherProxy)
        {
            this.Templates = Enum.GetValues<TemplateSelectorEnum>();
        }

        public IReadOnlyCollection<TemplateSelectorEnum> Templates { get; }
    }
}
