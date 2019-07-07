using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);
        void Insert(TEntity item);
        void Update(TEntity item);
        void Delete(int id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
    }
}
