using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Wonderlust.Core;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    class DefaultActionInteractor
    {
        IWorkspace workspace;
        IEnumerable<IWorkspaceItem> workspaceItems;

        public DefaultActionInteractor(IWorkspace workspace, IEnumerable<IWorkspaceItem> workspaceItems)
        {
            this.workspace = workspace;
            this.workspaceItems = workspaceItems;
        }

        public void Exec()
        {
            foreach (var item in workspaceItems)
            {
                if (item is ContainerWorkspaceItem containerItem)
                {   
                    workspace.SetContainer(containerItem.Container, false);
                    workspace.History.Add();
                }
                else
                {
                    if (item.PhysicalPath != null)
                    {
                        try
                        {
                            var psi = new ProcessStartInfo();
                            psi.FileName = item.PhysicalPath;
                            psi.UseShellExecute = true;

                            Process.Start(psi);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }
}
