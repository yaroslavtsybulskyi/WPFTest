namespace Logistic.ConsoleClient.Repositories
{
    public interface IRepository<TEntity>
    {
        TEntity Create(TEntity entity);
        IEnumerable<TEntity> ReadAll();
        TEntity ReadById(object id);
        void Update(object id, TEntity entity);
        void Delete(object id);
    }


}

