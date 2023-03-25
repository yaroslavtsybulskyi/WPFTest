using System;
using System.Collections.Generic;
using System.Linq;

namespace Logistic.ConsoleClient.Repositories
{

    public class InMemoryRepository<TEntity> : IRepository<TEntity>
    {
        private readonly List<TEntity> _entities = new List<TEntity>();
        private readonly Func<TEntity, object> _getIdFunc;

        public InMemoryRepository(Func<TEntity, object> getIdFunc)
        {
            _getIdFunc = getIdFunc;
        }

        public TEntity Create(TEntity entity)
        {
            _entities.Add(entity);
            return entity;
        }

        public IEnumerable<TEntity> ReadAll()
        {
            return _entities;
        }

        public TEntity ReadById(object id)
        {
            return _entities.Find(e => _getIdFunc(e).Equals(id));
        }

        public void Update(object id, TEntity entity)
        {
            var index = _entities.FindIndex(e => _getIdFunc(e).Equals(id));
            if (index >= 0)
            {
                _entities[index] = entity;
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

