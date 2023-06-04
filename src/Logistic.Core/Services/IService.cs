using Logistic.Models;

namespace Logistic.Core.Services
{
    public interface IService<TEntity>
    {
        TEntity Create(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        void Delete(int id);
        void LoadCargo(Cargo cargo, int entityId);
        void UnloadCargo(Guid cargoId, int entityId);
        void Update(TEntity entity);

    }
}
