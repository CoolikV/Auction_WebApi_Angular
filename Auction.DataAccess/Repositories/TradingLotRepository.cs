using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Repositories
{
    public class TradingLotRepository : ITradingLotRepository
    {
        internal readonly IDataContext Database;
        internal readonly DbSet<TradingLot> dbSet;

        public TradingLotRepository(IDataContext context)
        {
            Database = context;
            dbSet = context.Set<TradingLot>();
        }

        public IQueryable<TradingLot> TradingLots => dbSet;

        public void AddTradingLot(TradingLot tradingLot)
        {
            dbSet.Add(tradingLot);
        }

        public void DeleteTradingLot(TradingLot tradingLot)
        {
            if (Database.Entry(tradingLot).State == EntityState.Detached)
                dbSet.Attach(tradingLot);

            dbSet.Remove(tradingLot);
        }

        public void DeleteTradingLotById(int id)
        {
            TradingLot tradingLot = dbSet.Find(id);
            DeleteTradingLot(tradingLot);
        }

        public IEnumerable<TradingLot> FindTradingLots(Expression<Func<TradingLot, bool>> filter = null,
            Func<IQueryable<TradingLot>, IOrderedQueryable<TradingLot>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TradingLot> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public TradingLot GetTradingLotById(int id)
        {
            return dbSet.Find(id);
        }

        public void UpdadeTradingLot(TradingLot tradingLot)
        {
            dbSet.Attach(tradingLot);
            Database.Entry(tradingLot).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
