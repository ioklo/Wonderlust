namespace Wonderlust.Core.Abstractions
{
    public interface IStorageRepo
    {
        IStorage? GetStorage(StorageId storageId);
    }
}