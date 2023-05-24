namespace Logistic.DAL
{
    public interface IRepository<TEntity>
    {
        TEntity Create(TEntity entity);
        IEnumerable<TEntity> ReadAll();
        TEntity ReadById(int id);
        void Update(int id, TEntity entity);
        void Delete(int id);
    }
}