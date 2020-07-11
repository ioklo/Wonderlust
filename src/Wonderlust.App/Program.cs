using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wonderlust.Core;
using Wonderlust.Core.Abstractions;
using Wonderlust.WPF.ViewModels;
using Wonderlust.WPF.Views;

namespace Wonderlust.App
{
    class Program
    {
        // Entry point method
        [STAThread]
        public static void Main()
        {
            WPF.App app = new WPF.App();

            var driveContainer = new DriveContainer(@"Z:\Temp", Directory.GetLastWriteTime(@"Z:\Temp"));
            var categoryFactory = new CategoryFactory();
            var workspaceItemFactory = new WorkspaceItemFactory(categoryFactory);
            var workspace = new Workspace(workspaceItemFactory);

            workspace.SetSortOrder(EWorkspaceItemSortOrder.CategoryAscending);
            workspace.SetContainer(driveContainer, true);
            workspace.History.Add();

            // view
            var mainWindowVM = new MainWindowVM(workspace);
            mainWindowVM.OnExitRequested += () => { app.Shutdown(); };
            var mainWindow = new MainWindow(mainWindowVM);

            app.Run(mainWindow);
        }
    }
}
