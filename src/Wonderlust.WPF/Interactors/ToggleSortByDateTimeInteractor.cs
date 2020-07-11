using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    class ToggleSortByDateTimeInteractor : ToggleSortInterator
    {
        public ToggleSortByDateTimeInteractor(IWorkspace workspace)
            : base(workspace, EWorkspaceItemSortOrder.DateDescending, EWorkspaceItemSortOrder.DateAscending)
        {
        }
    }
}
