using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderlust.Core.Abstractions
{
    public interface ICategoryFactory
    {
        Category DirectoryCategory { get; }
        Category GetCategory(IContainerItem item);
    }
}
