using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderlust.Core.Abstractions
{
    // 작업 단위, 현재 위치를 갖고 있다
    public interface IWorkspace
    {
        IEnumerable<IWorkspaceItem> GetItems();
        void SetContainer(IContainer container);
        IContainer GetContainer();

        event Action<IWorkspace> OnContainerChanged;

        bool IsRelatedPrevContainer(IWorkspaceItem item);
    }
}
