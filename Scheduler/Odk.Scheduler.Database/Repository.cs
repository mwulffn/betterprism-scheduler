using PetaPoco;

namespace Odk.Scheduler.Database
{
    class Repository<TKey, TObject> : IRepository<TKey, TObject> where TObject : class
    {
        protected readonly IDatabase database;

        public Repository(PetaPoco.IDatabase database)
        {
            this.database = database;
        }

        public void Delete(TObject obj)
        {
            database.Delete<TObject>(obj);
        }

        public virtual void Insert(TObject obj)
        {
            database.Insert(obj);
        }

        public virtual void Save(TObject obj)
        {
            database.Save(obj);
        }

        public TObject Single(TKey key)
        {
            return database.Single<TObject>(key);
        }

        public TObject SingleOrDefault(TKey key)
        {
            return database.SingleOrDefault<TObject>(key);
        }        
    }
}