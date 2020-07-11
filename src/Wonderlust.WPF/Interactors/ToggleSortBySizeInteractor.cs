using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    class ToggleSortBySizeInteractor : ToggleSortInterator
    {
        public ToggleSortBySizeInteractor(IWorkspace workspace)
            : base(workspace, EWorkspaceItemSortOrder.SizeDescending, EWorkspaceItemSortOrder.SizeAscending)
        {
        }
    }
}
