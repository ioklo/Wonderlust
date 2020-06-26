using System.Collections.Generic;

namespace Wonderlust.Core.Abstractions
{
    // 저장장치 단위 (파일시스템, 원드라이브)
    public interface IStorage
    {
        StorageId StorageId { get; }
        IEnumerable<IItem> GetItems(string path);
    }    
}