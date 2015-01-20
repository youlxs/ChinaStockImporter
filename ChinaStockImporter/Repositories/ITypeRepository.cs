namespace Yootech.ChinaStockImporter.Repositories
{
    public interface ITypeRepository
    {
        void Dispose();
        bool Save();
        void UpdateOrInsert(int fileId, object entity);
        void AutoDetectChanges(bool autoDetectChanges = true);
    }
}
