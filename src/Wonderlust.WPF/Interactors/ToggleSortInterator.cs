using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    abstract class ToggleSortInterator
    {
        IWorkspace workspace;
        EWorkspaceItemSortOrder first;
        EWorkspaceItemSortOrder second;

        protected ToggleSortInterator(IWorkspace workspace, EWorkspaceItemSortOrder first, EWorkspaceItemSortOrder second)
        {
            this.workspace = workspace;
            this.first = first;
            this.second = second;
        }

        public void Exec()
        {
            var order = workspace.GetSortOrder();

            if (order == first)
                workspace.SetSortOrder(second);
            else
                workspace.SetSortOrder(first);

            workspace.Sort();
        }
    }
}