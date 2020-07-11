using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    class ToggleSortByNameInteractor : ToggleSortInterator
    {
        public ToggleSortByNameInteractor(IWorkspace workspace)
            : base(workspace, EWorkspaceItemSortOrder.NameAscending, EWorkspaceItemSortOrder.NameDescending)
        {
        }
    }
}
