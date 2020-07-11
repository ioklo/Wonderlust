using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    class SetSortNoneInterator
    {
        IWorkspace workspace;

        public SetSortNoneInterator(IWorkspace workspace)
        {
            this.workspace = workspace;
        }

        public void Exec()
        {
            if (workspace.GetSortOrder() != EWorkspaceItemSortOrder.None)
            {
                workspace.SetSortOrder(EWorkspaceItemSortOrder.None);
                workspace.Sort();
            }
        }
    }
}
