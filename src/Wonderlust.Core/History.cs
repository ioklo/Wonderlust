using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    class HistoryEntry
    {
        public IContainer Container { get; }
        public IWorkspaceItem WorkspaceItem { get; }

        public HistoryEntry(IContainer container, IWorkspaceItem workspaceItem)
        {
            Container = container;
            WorkspaceItem = workspaceItem;
        }
    }

    public interface IHistoryWorkspace
    {
        IContainer? GetContainer();
        IWorkspaceItem? GetCurItem();

        void SetContainer(IContainer container, IWorkspaceItem workspaceItem);
    }

    // 워크스페이스별로 히스토리를 관리해야 한다
    public class History : IHistory
    {
        IHistoryWorkspace workspace;
        List<HistoryEntry> entries;
        int index;

        public History(IHistoryWorkspace workspace)
        {
            this.workspace = workspace;

            entries = new List<HistoryEntry>();
            index = 0;
        }

        public void Add()
        {
            // 현재 위치(Container, IWorkspaceItem)
            var container = workspace.GetContainer();
            var item = workspace.GetCurItem();

            if (container == null) return;
            if (item == null) return;

            // 0 1 2 3 4 Count:5
            //     ^  
            // 0 1 2 3' 
            //       ^            
            if (index < entries.Count - 1)
                entries.RemoveRange(index + 1, entries.Count - index - 1);

            entries.Add(new HistoryEntry(container, item));
            index = entries.Count - 1;
        }

        public void Back()
        {
            if (index - 1 < 0) return;

            index--;
            workspace.SetContainer(entries[index].Container, entries[index].WorkspaceItem);
        }

        public void Forward()
        {
            if (entries.Count <= index + 1) return;

            index++;
            workspace.SetContainer(entries[index].Container, entries[index].WorkspaceItem);
        }
    }
}
