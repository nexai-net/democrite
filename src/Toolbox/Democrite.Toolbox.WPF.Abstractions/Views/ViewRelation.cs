// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.Abstractions.Views
{
    using System;

    /// <summary>
    /// View relation between view and view model
    /// </summary>
    public sealed record ViewRelation(Guid Uid, Type View, Type ViewModel, ViewRelationInfo Info);
}
