using System;
using System.Drawing;

namespace Wonderlust.Core.Abstractions
{
    public interface IWorkspaceItem
    {
        string DisplayName { get; }
        long? Size { get; }
        DateTime? DateTime { get; }

        Category Category { get; }
        void Exec();
    }
}