using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderlust.Core.Abstractions
{
    // 임의의 저장장치의 한 뷰 (디렉토리)
    public interface IContainer : IEquatable<IContainer>
    {
        string Name { get; }
        DateTime? DateTime { get; }
        string? PhysicalPath { get; }

        IContainer? GetParent();
        IEnumerable<IContainerItem> GetItems();        
    }
}
