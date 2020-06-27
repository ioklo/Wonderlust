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
            
            var driveContainer = new DriveContainer(@"Z:\Temp");
            var workspace = new Workspace(driveContainer);

            // view
            var mainWindow = new MainWindow(new MainWindowVM(workspace));

            app.Run(mainWindow);
        }
    }
}
