using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class Workspace : IWorkspace
    {
        IContainer? prevContainer;
        IContainer container;
        IWorkspaceItemFactory itemFactory;

        public event Action<IWorkspace>? OnContainerChanged;

        public Workspace(IContainer container, IWorkspaceItemFactory itemFactory)
        {
            this.prevContainer = null;
            this.container = container;
            this.itemFactory = itemFactory;
        }

        public (IEnumerable<IWorkspaceItem> Items, IWorkspaceItem? InitialSelection) GetItems()
        {
            var items = new List<IWorkspaceItem>();
            IWorkspaceItem? initialSelection = null;

            var parent = container.GetParent();
            if (parent != null)
                items.Add(itemFactory.MakeParentDirectoryItem(this, parent));

            try
            {
                foreach (var item in container.GetItems())
                {
                    if (item is IDirectoryContainerItem directoryItem)
                    {
                        var wi = itemFactory.MakeDirectoryItem(this, directoryItem.Container);

                        // TODO: 지금은 디렉토리만 지원한다, 하지만 prevContainer는 디렉토리가 아닐 수도 있다(zip)
                        if (directoryItem.Container.Equals(prevContainer))
                            initialSelection = wi;

                        items.Add(wi);
                    }
                    else
                        items.Add(itemFactory.Make(this, item));
                }
            }
            catch
            {

            }

            return (items, initialSelection);
        }

        public void SetContainer(IContainer newContainer, bool bDontSetInitialSelection)
        {
            prevContainer = bDontSetInitialSelection ? null : container;

            container = newContainer;
            OnContainerChanged?.Invoke(this);
        }

        public IContainer GetContainer()
        {
            return container;
        }
        
        public void SetContainerToParent()
        {
            var parent = container.GetParent();

            if (parent != null)
                SetContainer(parent, false);
        }
    }
}
