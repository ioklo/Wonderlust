using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderlust.Core.Abstractions
{
    public interface IContainerItem
    {
        string Name { get; }
        long? Size { get; }
        DateTime? DateTime { get; }
    }
}
