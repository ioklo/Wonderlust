using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class WorkspaceItemFactory : IWorkspaceItemFactory
    {
        ICategoryFactory categoryFactory;

        public WorkspaceItemFactory(ICategoryFactory categoryFactory)
        {
            this.categoryFactory = categoryFactory;
        }

        public IWorkspaceItem MakeParentDirectoryItem(IWorkspace workspace, IContainer container)
        {
            return new ParentDirectoryWorkspaceItem(categoryFactory.DirectoryCategory, workspace, container);
        }

        public IWorkspaceItem MakeDirectoryItem(IWorkspace workspace, IContainer container)
        {
            return new DirectoryWorkspaceItem(categoryFactory.DirectoryCategory, workspace, container);
        }

        public IWorkspaceItem Make(IWorkspace workspace, IContainerItem item)
        {
            var category = categoryFactory.GetCategory(item);

            if (item is FileContainerItem fileItem)
            {
                return new FileWorkspaceItem(category, fileItem);
            }
            
            return new DummyWorkspaceItem(category, item);
        }
    }
}
