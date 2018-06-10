using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Infastructure
{
    public interface IRepository<TEntity> where TEntity : class 
    {
        // New entity
        void Add(TEntity entity);
        // Modify entity
        void Update(TEntity entity);
        // Remove entity
        void Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> where);
        // Get an entity by id
        TEntity GetById(int id);
        TEntity Get(Expression<Func<TEntity, bool>> where);
        // Get all entities of type TClass
        IEnumerable<TEntity> GetAll();
        // Get entites using delegate
        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);
    }
}
