using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    class DirectoryWorkspaceItem : ContainerWorkspaceItem
    {
        public override string DisplayName => Container.Name;

        public DirectoryWorkspaceItem(Category category, IWorkspace workspace, IContainer container)
            : base(category, workspace, container)
        {
        }
    }
}
