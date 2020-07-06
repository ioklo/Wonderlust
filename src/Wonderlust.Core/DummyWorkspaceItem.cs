using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class DummyWorkspaceItem : WorkspaceItem
    {
        IContainerItem item;
        public override string DisplayName => item.Name;

        public override long? Size => item.Size;

        public override DateTime? DateTime => item.DateTime;

        public override string? PhysicalPath => null;

        public DummyWorkspaceItem(Category category, IContainerItem item)
            : base(category)
        {
            this.item = item;
        }


    }
}
