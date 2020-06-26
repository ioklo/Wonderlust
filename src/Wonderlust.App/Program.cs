using System;
using System.Collections.Generic;
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

            // view
            var mainWindow = new MainWindow();

            var storageRepo = new StorageRepo();
            var driveStorage = storageRepo.AddStorage(id => new DriveStorage(id));

            var storagePath = new StoragePath(driveStorage.StorageId, @"Z:\\Temp");
            var workspace = new Workspace(storageRepo, storagePath);

            // viewModel
            mainWindow.DataContext = new MainWindowVM(workspace);

            app.Run(mainWindow);
        }
    }
}
