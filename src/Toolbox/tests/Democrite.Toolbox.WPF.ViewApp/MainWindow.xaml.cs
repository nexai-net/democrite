// Copyright (c) Nexai.
// The Democrite licenses this file to you under the MIT license.
// Produce by nexai & community (cf. docs/Teams.md)

namespace Democrite.Toolbox.WPF.ViewApp
{
    using Democrite.Toolbox.WPF.UI.Avalon.Services;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(AvalonDockViewManager? avalonDockViewManager = null)
        {
            InitializeComponent();

            //this.avalonDock.Theme = new ToolBoxTheme();
            avalonDockViewManager?.RegisterDockManager(this.avalonDock);
        }
    }
}