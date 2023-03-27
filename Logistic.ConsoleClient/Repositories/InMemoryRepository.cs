using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Logistic.ConsoleClient.Models;

namespace Logistic.ConsoleClient.Repositories
{

    public class InMemoryRepository<TEntity> : IRepository<TEntity>
    {
        private readonly List<TEntity> _entities = new List<TEntity>();
        private readonly Func<TEntity, object> _getIdFunc;
        private readonly IMapper _mapper;
        protected int lastId = 0;

        public InMemoryRepository(Func<TEntity, object> getIdFunc)
        {
            _getIdFunc = getIdFunc;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TEntity, TEntity>();
            });

            _mapper = config.CreateMapper();
        }

        public TEntity Create(TEntity entity)
        {

            if (entity is IEntity entityWithId)
            {
                entityWithId.Id = ++lastId;
            }
            var newEntity = _mapper.Map<TEntity>(entity);
            _entities.Add(newEntity);
            return newEntity;
        }

        public IEnumerable<TEntity> ReadAll()
        {
            var entitiesToReturn = _mapper.Map<IEnumerable<TEntity>, IEnumerable<TEntity>>(_entities);
            return entitiesToReturn;
        }

        public TEntity ReadById(object id)
        {
            return _entities.Find(e => _getIdFunc(e).Equals(id));
        }

        public void Update(object id, TEntity entity)
        {
            var existingEntity = _entities.FirstOrDefault(e => _getIdFunc(e).Equals(id));
            if (existingEntity != null)
            {
                _entities.Remove(existingEntity);
                _entities.Add(entity);
            }
        }

        public void Delete(object id)
        {
            var entity = _entities.Find(e => _getIdFunc(e).Equals(id));
            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }
    }


}

