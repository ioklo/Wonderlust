using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class DummyWorkspaceItem : IWorkspaceItem
    {
        IContainerItem item;
        public string DisplayName => item.Name;

        public Color Color => Color.FromArgb(167, 167, 167);

        public DummyWorkspaceItem(IContainerItem item)
        {
            this.item = item;
        }

        public void Exec()
        {
        }
    }
}
