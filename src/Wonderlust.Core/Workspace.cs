using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class Workspace : IWorkspace
    {
        IStorageRepo storageRepo;
        public StoragePath CurrentPath { get; }

        public Workspace(IStorageRepo storageRepo, StoragePath curPath)
        {
            this.storageRepo = storageRepo;
            CurrentPath = curPath;
        }

        public IEnumerable<IItem> GetItems()
        {
            var storage = storageRepo.GetStorage(CurrentPath.StorageId);
            if (storage == null)
                return Enumerable.Empty<IItem>();

            return storage.GetItems(CurrentPath.Path);
        }
    }
}
