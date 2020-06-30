using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderlust.Core.Abstractions
{
    public interface IDirectoryContainerItem : IContainerItem
    {
        IContainer Container { get; }
    }
}
