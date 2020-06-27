using System.Drawing;

namespace Wonderlust.Core.Abstractions
{
    public interface IWorkspaceItem
    {
        string DisplayName { get; }
        Color Color { get; }
        void Exec();
    }
}