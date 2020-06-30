using System;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    abstract class ContainerWorkspaceItem : WorkspaceItem
    {
        IWorkspace workspace;
        public IContainer Container { get; }

        public override long? Size => null;
        public override DateTime? DateTime => Container.DateTime;

        public ContainerWorkspaceItem(Category category, IWorkspace workspace, IContainer container)
            : base(category)
        {
            this.workspace = workspace;
            Container = container;
        }

        public override void Exec()
        {
            workspace.SetContainer(Container);
        }
    }
}
