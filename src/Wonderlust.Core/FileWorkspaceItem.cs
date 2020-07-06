using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class FileWorkspaceItem : WorkspaceItem
    {
        public FileContainerItem Item { get; }
        public override string DisplayName => Item.Name;

        public override long? Size => Item.Size;

        public override DateTime? DateTime => Item.DateTime;

        public override string? PhysicalPath => Item.Path;

        public FileWorkspaceItem(Category category, FileContainerItem item)
            : base(category)
        {
            this.Item = item;
        }
    }
}
