using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class StorageRepo : IStorageRepo
    {
        Dictionary<StorageId, IStorage> storages;

        public StorageRepo()
        {
            storages = new Dictionary<StorageId, IStorage>();
        }

        public IStorage AddStorage(Func<StorageId, IStorage> storageConstructor)
        {
            var id = new StorageId(storages.Count);

            var storage = storageConstructor.Invoke(id);
            Debug.Assert(storage.StorageId == id, "Storage constructor must set its id to given storage id argument.");

            storages.Add(id, storage);
            return storage;
        }

        public IStorage? GetStorage(StorageId storageId)
        {
            if (storages.TryGetValue(storageId, out var storage))
                return storage;

            return null;
        }
    }
}
