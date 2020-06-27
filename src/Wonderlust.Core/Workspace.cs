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

        public event Action<IWorkspace>? OnContainerChanged;

        public Workspace(IContainer container)
        {
            this.prevContainer = null;
            this.container = container;
        }

        public IEnumerable<IWorkspaceItem> GetItems()
        {
            var parent = container.GetParent();
            if (parent != null)
                yield return new ParentDirectoryWorkspaceItem(this, parent);

            foreach(var item in container.GetItems())
            {
                if (item is DirectoryContainerItem directoryItem)
                    yield return new DirectoryWorkspaceItem(this, directoryItem);
                //else if (storageItem is FileItem fileItem)
                //    yield return new FileWorkspaceItem(this, fileItem);
                else
                    yield return new DummyWorkspaceItem(item);
            }
        }

        public void SetContainer(IContainer newContainer)
        {
            prevContainer = container;
            container = newContainer;
            OnContainerChanged?.Invoke(this);
        }

        public IContainer GetContainer()
        {
            return container;
        }

        public bool IsRelatedPrevContainer(IWorkspaceItem item)
        {
            if (prevContainer == null) return false;

            // TODO: 지금은 Drive만 검사한다
            if (prevContainer is DriveContainer driveContainer && item is DirectoryWorkspaceItem dwi)
                if (driveContainer.Path == dwi.Item.Path)
                    return true;

            return false;

        }

        
    }
}
