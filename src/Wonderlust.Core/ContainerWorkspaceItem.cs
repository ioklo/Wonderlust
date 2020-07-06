using System;
using System.Diagnostics;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public abstract class ContainerWorkspaceItem : WorkspaceItem 
    {
        IWorkspace workspace;
        public IContainer Container { get; }

        public override long? Size => null;
        public override DateTime? DateTime => Container.DateTime;
        public override string? PhysicalPath => Container.PhysicalPath;

        public ContainerWorkspaceItem(Category category, IWorkspace workspace, IContainer container)
            : base(category)
        {
            this.workspace = workspace;
            Container = container;
        }       
        
    }
}
