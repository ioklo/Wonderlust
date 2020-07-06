using System;
using System.Diagnostics;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    abstract class ContainerWorkspaceItem : WorkspaceItem
    {
        IWorkspace workspace;
        public IContainer Container { get; }

        public override long? Size => null;
        public override DateTime? DateTime => Container.DateTime;

        public ContainerWorkspaceItem(Category category, IWorkspace workspace, IContainer container)
            : base(category)
        {
            this.workspace = workspace;
            Container = container;
        }

        public override void Exec()
        {
            workspace.SetContainer(Container, false);
        }

        public override void ShowProperties()
        {
            try
            {
                if (Container is DriveContainer driveContainer)
                {
                    PropertyWindow.Open(driveContainer.Path);

                    //var psi = new ProcessStartInfo();
                    //psi.FileName = driveContainer.Path;
                    //psi.UseShellExecute = true;
                    //psi.Verb = "properties";

                    //Process.Start(psi);
                }
            }
            catch
            {

            }
        }
    }
}
