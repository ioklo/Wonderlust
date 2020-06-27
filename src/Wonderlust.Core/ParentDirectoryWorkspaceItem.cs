using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    class ParentDirectoryWorkspaceItem : IWorkspaceItem
    {
        IWorkspace workspace;
        IContainer container;

        public string DisplayName => "..";

        public Color Color => Color.FromArgb(251, 83, 83);

        public ParentDirectoryWorkspaceItem(IWorkspace workspace, IContainer container)
        {
            this.workspace = workspace;
            this.container = container;
        }

        public void Exec()
        {
            workspace.SetContainer(container);
        }
    }
}
