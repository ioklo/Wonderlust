using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    class ParentDirectoryWorkspaceItem : ContainerWorkspaceItem
    {
        public override string DisplayName => "..";

        public ParentDirectoryWorkspaceItem(Category category, IWorkspace workspace, IContainer container)
            : base(category, workspace, container)
        {
        }
    }
}
