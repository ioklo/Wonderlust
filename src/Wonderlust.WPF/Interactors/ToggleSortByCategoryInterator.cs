using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    class ToggleSortByCategoryInteractor : ToggleSortInterator
    {
        public ToggleSortByCategoryInteractor(IWorkspace workspace)
            : base(workspace, EWorkspaceItemSortOrder.CategoryAscending, EWorkspaceItemSortOrder.CategoryDescending)
        {
        }        
    }
}
