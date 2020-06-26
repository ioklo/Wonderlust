using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{   
    public class DriveStorage : IStorage
    {
        public StorageId StorageId { get; }

        public DriveStorage(StorageId storageId)
        {
            StorageId = storageId;
        }

        public IEnumerable<IItem> GetItems(string path)
        {
            // 파일이 바뀔 수 있으므로 yield 하지 말고 바로 끝낸다
            return 
                Directory.EnumerateDirectories(path).Select(dir => new DriveDirectoryItem(dir)).Concat<IItem>(
                Directory.EnumerateFiles(path).Select(file => new DriveFileItem(file)));
        }
    }
}
