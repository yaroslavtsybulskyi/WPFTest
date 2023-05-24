using AutoMapper;
using Logistic.Models;

namespace Logistic.DAL
{
    public class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        private readonly List<TEntity> _entities = new List<TEntity>();
        private readonly Func<TEntity, object> _getIdFunc;
        private readonly IMapper _mapper;
        protected int lastId = 0;

        public InMemoryRepository()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TEntity, TEntity>();
            });

            _mapper = config.CreateMapper();
        }

        public TEntity Create(TEntity entity)
        {
            entity.Id = ++lastId;
            var newEntity = _mapper.Map<TEntity>(entity);
            _entities.Add(newEntity);
            return newEntity;
        }

        public IEnumerable<TEntity> ReadAll()
        {
            var entitiesToReturn = _mapper.Map<IEnumerable<TEntity>, IEnumerable<TEntity>>(_entities);
            return entitiesToReturn;
        }

        public TEntity ReadById(int id)
        {
            return _entities.FirstOrDefault(e => e.Id.Equals(id));
        }

        public void Update(int id, TEntity entity)
        {
            var existingEntity = _entities.FirstOrDefault(e => e.Id.Equals(id));
            if (existingEntity != null)
            {
                _entities.Remove(existingEntity);
                _entities.Add(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = _entities.FirstOrDefault(e => e.Id.Equals(id));
            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }
    }
}