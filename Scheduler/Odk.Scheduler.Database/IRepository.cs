namespace Odk.Scheduler.Database
{
    public interface IRepository<TKey, TObject> where TObject : class
    {
        TObject Single(TKey key);
        TObject SingleOrDefault(TKey key);
        void Insert(TObject obj);
        void Save(TObject obj);
        void Delete(TObject obj);
    }
}