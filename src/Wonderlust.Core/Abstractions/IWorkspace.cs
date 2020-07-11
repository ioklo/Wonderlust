using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderlust.Core.Abstractions
{
    public enum EWorkspaceItemSortOrder
    {
        None,
        CategoryAscending,
        CategoryDescending,
        NameAscending,
        NameDescending,
        DateAscending,
        DateDescending,
        SizeAscending,
        SizeDescending,
    }

    // 작업 단위, 현재 위치, 커서를 갖고 있다
    public interface IWorkspace
    {
        IHistory History { get; }

        IEnumerable<IWorkspaceItem> GetItems();
        IWorkspaceItem? GetCurItem();
        void SetCurItem(IWorkspaceItem? item);

        void SetContainer(IContainer container, IWorkspaceItem item);
        void SetContainer(IContainer container, bool bDontSetInitialSelection);
        
        IContainer? GetContainer();
        void SetContainerToParent(); // TODO: GetContainer가 있으니, 부모를 알아내서 세팅하는 것은 상위 레벨에서 할법한 행동이다

        void SetSortOrder(EWorkspaceItemSortOrder order);
        EWorkspaceItemSortOrder GetSortOrder();

        void Sort();

        event Action<IWorkspace> OnContainerChanged;
    }
}
