using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class Workspace : IWorkspace, IHistoryWorkspace
    {
        IWorkspaceItemFactory itemFactory;

        IContainer? container;
        List<IWorkspaceItem> items;
        IWorkspaceItem? curItem;
        History history;

        public IHistory History => history;

        public event Action<IWorkspace>? OnContainerChanged;

        public Workspace(IWorkspaceItemFactory itemFactory)
        {
            this.itemFactory = itemFactory;            

            container = null;
            items = new List<IWorkspaceItem>();
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

                // TODO: 아무것도 없으면 뭐라도 하나 만들어야 한다
                if (curItem == null && items.Count != 0)
                    curItem = items[0];
                
            }
            catch
            {

            }

            OnContainerChanged?.Invoke(this);
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
    }
}
