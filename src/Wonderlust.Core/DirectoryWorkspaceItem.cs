using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    class DirectoryWorkspaceItem : IWorkspaceItem
    {
        IWorkspace workspace;
        public DirectoryContainerItem Item { get; }
        public string DisplayName => Item.Name;

        public Color Color => Color.FromArgb(251, 83, 83);

        public DirectoryWorkspaceItem(IWorkspace workspace, DirectoryContainerItem item)
        {
            this.workspace = workspace;
            this.Item = item;
        }

        public void Exec()
        {
            workspace.SetContainer(new DriveContainer(Item.Path));
        }
    }
}
