using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderlust.Core.Abstractions
{
    // 작업 단위, 현재 위치를 갖고 있다
    public interface IWorkspace
    {
        IHistory History { get; }

        IEnumerable<IWorkspaceItem> GetItems();
        IWorkspaceItem? GetCurItem();
        void SetCurItem(IWorkspaceItem? item);

        void SetContainer(IContainer container, IWorkspaceItem item);
        void SetContainer(IContainer container, bool bDontSetInitialSelection);
        IContainer? GetContainer();

        event Action<IWorkspace> OnContainerChanged;
        
        void SetContainerToParent();        
    }
}
