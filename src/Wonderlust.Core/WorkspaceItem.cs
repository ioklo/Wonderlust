using System;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public abstract class WorkspaceItem : IWorkspaceItem
    {
        public Category Category { get; }
        public abstract string DisplayName { get; }
        public abstract long? Size { get; }
        public abstract DateTime? DateTime { get; }

        public WorkspaceItem(Category category)
        {
            Category = category;
        }

        public abstract void Exec();

        public abstract void ShowProperties();
    }
}