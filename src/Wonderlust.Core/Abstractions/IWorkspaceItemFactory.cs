using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderlust.Core.Abstractions
{
    public interface IWorkspaceItemFactory
    {
        IWorkspaceItem MakeParentDirectoryItem(IWorkspace workspace, IContainer container);
        IWorkspaceItem MakeDirectoryItem(IWorkspace workspace, IContainer container);
        IWorkspaceItem Make(IWorkspace workspace, IContainerItem containerItem);
    }
}
