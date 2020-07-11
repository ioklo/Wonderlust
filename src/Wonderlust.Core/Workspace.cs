using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class Workspace : IWorkspace, IHistoryWorkspace
    {
        IWorkspaceItemFactory itemFactory;

        IContainer? container;
        List<IWorkspaceItem> itemsNoOrder;
        List<IWorkspaceItem> items;
        IWorkspaceItem? curItem;
        History history;

        // TODO: 옵션을 여기에 저장해야 하는건 아닐 것 같다
        EWorkspaceItemSortOrder sortOrder;

        public IHistory History => history;

        public event Action<IWorkspace>? OnContainerChanged;

        public Workspace(IWorkspaceItemFactory itemFactory)
        {
            this.itemFactory = itemFactory;            

            container = null;
            items = new List<IWorkspaceItem>();
            itemsNoOrder = new List<IWorkspaceItem>();
            curItem = null;

            history = new History(this);
        }

        public IEnumerable<IWorkspaceItem> GetItems()
        {
            return items;
        }

        public void SetContainer(IContainer container, IWorkspaceItem workspaceItem)
        {
            if (workspaceItem is ContainerWorkspaceItem containerItem)
            {
                SetContainer(container, item =>
                {
                    if( item is DirectoryContainerItem directoryItem)
                        return containerItem.Container.Equals(directoryItem.Container);

                    return false;
                });
            }
            else if (workspaceItem is FileWorkspaceItem fileItem)
            {
                SetContainer(container, item =>
                {
                    if (item is FileContainerItem fileContainerItem)
                        return fileItem.Item.Path == fileContainerItem.Path;

                    return false;
                });
            }
            else
            {
                SetContainer(container, item => false);
            }
        }

        public void SetContainer(IContainer container, bool bDontSetInitialSelection)
        {
            if (bDontSetInitialSelection)
            {
                SetContainer(container, item => false);
            }
            else
            {
                var prevContainer = this.container;

                SetContainer(container, item =>
                {
                    // TODO: 지금은 디렉토리만 지원한다, 하지만 prevContainer는 디렉토리가 아닐 수도 있다(zip)
                    if (item is IDirectoryContainerItem directoryItem)
                        return directoryItem.Container.Equals(prevContainer);

                    return false;
                });
            }
        }

        public void SetContainer(IContainer newContainer, Predicate<IContainerItem> isInitialSelection)
        {
            container = newContainer;            

            items.Clear();
            curItem = null;

            var parent = container.GetParent();
            if (parent != null)
                items.Add(itemFactory.MakeParentDirectoryItem(this, parent));

            try
            {
                foreach (var item in container.GetItems())
                {
                    IWorkspaceItem wi;

                    if (item is IDirectoryContainerItem directoryItem)
                        wi = itemFactory.MakeDirectoryItem(this, directoryItem.Container);
                    else
                        wi = itemFactory.Make(this, item);

                    items.Add(wi);

                    if (curItem == null && isInitialSelection.Invoke(item))
                    {
                        curItem = wi;
                    }
                }

                itemsNoOrder.Clear();
                itemsNoOrder.AddRange(items);

                // 소트
                SortItems();

                // TODO: 아무것도 없으면 뭐라도 하나 만들어야 한다
                if (curItem == null && items.Count != 0)
                    curItem = items[0];
                
            }
            catch
            {

            }

            OnContainerChanged?.Invoke(this);
        }

        private int? CompareInner(IWorkspaceItem x, IWorkspaceItem y)
        {
            // 1. ParentDirectory 이면 
            if (x is ParentDirectoryWorkspaceItem)
            {
                if (y is ParentDirectoryWorkspaceItem)
                    return 0;

                return -1;
            }
            else if (y is ParentDirectoryWorkspaceItem)
            {
                return 1;
            }

            // 2. Directory이면
            if (x is DirectoryWorkspaceItem dirX)
            {
                if (y is DirectoryWorkspaceItem dirY)
                    return string.Compare(dirX.DisplayName, dirY.DisplayName, StringComparison.CurrentCultureIgnoreCase);

                return -1;
            }
            else if (y is DirectoryWorkspaceItem)
            {
                return 1;
            }

            return null;
        }

        // 2 < 3 => 2 - 3 < 0, 1에서 2를 뺀다
        private int CompareByCategoryAscending(IWorkspaceItem x, IWorkspaceItem y)
        {
            var innerResult = CompareInner(x, y);
            if (innerResult.HasValue) return innerResult.Value;
            
            // 3. 나머지 끼리 카테고리 비교
            if (x.Category.Priority != y.Category.Priority)
                return x.Category.Priority - y.Category.Priority;

            if (x.Category.DefOrder != y.Category.DefOrder)
                return x.Category.DefOrder - y.Category.DefOrder;

            return string.Compare(x.DisplayName, y.DisplayName, StringComparison.CurrentCultureIgnoreCase);            
        }

        private int CompareByCategoryDescending(IWorkspaceItem x, IWorkspaceItem y)
        {
            var innerResult = CompareInner(x, y);
            if (innerResult.HasValue) return innerResult.Value;

            if (x.Category.Priority != y.Category.Priority)
                return y.Category.Priority - x.Category.Priority;

            if (x.Category.DefOrder != y.Category.DefOrder)
                return y.Category.DefOrder - x.Category.DefOrder;

            // ascending에 영향 받지 않는다.
            return string.Compare(x.DisplayName, y.DisplayName, StringComparison.CurrentCultureIgnoreCase);
        }

        private int CompareBySizeAscending(IWorkspaceItem x, IWorkspaceItem y)
        {
            var innerResult = CompareInner(x, y);
            if (innerResult.HasValue) return innerResult.Value;

            // 3. 나머지 끼리 사이즈 비교
            if (!x.Size.HasValue)
            {
                if (!y.Size.HasValue)
                    return 0;

                // null < 2233 
                return -1;
            }
            else if (!y.Size.HasValue)
            {
                // 2233 > null
                return 1;
            }

            long v = x.Size.Value - y.Size.Value;

            if (v == 0L)
                return string.Compare(x.DisplayName, y.DisplayName, StringComparison.CurrentCultureIgnoreCase);

            return v < 0L ? -1 : 1;
        }

        private int CompareBySizeDescending(IWorkspaceItem x, IWorkspaceItem y)
        {
            var innerResult = CompareInner(x, y);
            if (innerResult.HasValue) return innerResult.Value;

            // 3. 나머지 끼리 사이즈 비교
            if (!x.Size.HasValue)
            {
                if (!y.Size.HasValue)
                    return 0;

                // null > 2233 
                return 1;
            }
            else if (!y.Size.HasValue)
            {
                // 2233 < null
                return -1;
            }

            long v = x.Size.Value - y.Size.Value;

            if (v == 0L)
                return string.Compare(x.DisplayName, y.DisplayName, StringComparison.CurrentCultureIgnoreCase);

            return v < 0L ? 1 : -1;
        }

        private int CompareByDateAscending(IWorkspaceItem x, IWorkspaceItem y)
        {
            var innerResult = CompareInner(x, y);
            if (innerResult.HasValue) return innerResult.Value;

            // 3. 나머지 끼리 시간 비교
            if (!x.DateTime.HasValue)
            {
                if (!y.DateTime.HasValue)
                    return 0;

                // null < 2020-07-11
                return -1;
            }
            else if (!y.DateTime.HasValue)
            {
                // 2020-07-11 > null
                return 1;
            }

            if (x.DateTime.Value == y.DateTime.Value)
                return string.Compare(x.DisplayName, y.DisplayName, StringComparison.CurrentCultureIgnoreCase);

            return x.DateTime.Value < y.DateTime.Value ? -1 : 1;
        }

        private int CompareByDateDescending(IWorkspaceItem x, IWorkspaceItem y)
        {
            var innerResult = CompareInner(x, y);
            if (innerResult.HasValue) return innerResult.Value;

            // 3. 나머지 끼리 시간 비교
            if (!x.DateTime.HasValue)
            {
                if (!y.DateTime.HasValue)
                    return 0;

                // null > 2020-07-11
                return 1;
            }
            else if (!y.DateTime.HasValue)
            {
                // 2020-07-11 < null
                return -1;
            }

            if (x.DateTime.Value == y.DateTime.Value)
                return string.Compare(x.DisplayName, y.DisplayName, StringComparison.CurrentCultureIgnoreCase);

            return x.DateTime.Value < y.DateTime.Value ? 1 : -1;
        }

        private int CompareByNameAscending(IWorkspaceItem x, IWorkspaceItem y)
        {
            var innerResult = CompareInner(x, y);
            if (innerResult.HasValue) return innerResult.Value;

            return string.Compare(x.DisplayName, y.DisplayName, StringComparison.CurrentCultureIgnoreCase);
        }

        private int CompareByNameDescending(IWorkspaceItem x, IWorkspaceItem y)
        {
            var innerResult = CompareInner(x, y);
            if (innerResult.HasValue) return innerResult.Value;

            return string.Compare(y.DisplayName, x.DisplayName, StringComparison.CurrentCultureIgnoreCase);
        }


        private void SortItems()
        {
            switch(sortOrder)
            {
                case EWorkspaceItemSortOrder.None:
                    items.Clear();
                    items.AddRange(itemsNoOrder);
                    break;

                case EWorkspaceItemSortOrder.CategoryAscending:
                    items.Sort(CompareByCategoryAscending);
                    break;

                case EWorkspaceItemSortOrder.CategoryDescending:
                    items.Sort(CompareByCategoryDescending);
                    break;

                case EWorkspaceItemSortOrder.NameAscending:
                    items.Sort(CompareByNameAscending);
                    break;

                case EWorkspaceItemSortOrder.NameDescending:
                    items.Sort(CompareByNameDescending);
                    break;

                case EWorkspaceItemSortOrder.DateAscending:
                    items.Sort(CompareByDateAscending);
                    break;

                case EWorkspaceItemSortOrder.DateDescending:
                    items.Sort(CompareByDateDescending);
                    break;

                case EWorkspaceItemSortOrder.SizeAscending:
                    items.Sort(CompareBySizeAscending);
                    break;

                case EWorkspaceItemSortOrder.SizeDescending:
                    items.Sort(CompareBySizeDescending);
                    break;

                default:
                    throw new NotImplementedException("구현되지 않은 소트방식입니다");
            }
        }

        
        public IContainer? GetContainer()
        {
            return container;
        }
        
        public void SetContainerToParent()
        {
            if (container == null) return;

            var parent = container.GetParent();

            if (parent != null)
                SetContainer(parent, false);
        }

        public void SetCurItem(IWorkspaceItem? workspaceItem)
        {
            curItem = workspaceItem;
        }

        public IWorkspaceItem? GetCurItem()
        {
            return curItem;
        }

        public void SetSortOrder(EWorkspaceItemSortOrder order)
        {
            sortOrder = order;
        }

        public EWorkspaceItemSortOrder GetSortOrder()
        {
            return sortOrder;
        }

        public void Sort()
        {
            SortItems();
            OnContainerChanged?.Invoke(this);
        }
    }
}
