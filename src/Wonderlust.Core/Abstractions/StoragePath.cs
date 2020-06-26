using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public struct StoragePath
    {
        public StorageId StorageId { get; }
        public string Path { get; }

        public StoragePath(StorageId storageId, string path)
        {
            StorageId = storageId;
            Path = path;
        }
    }
}
