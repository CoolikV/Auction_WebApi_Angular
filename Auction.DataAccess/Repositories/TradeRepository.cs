using Auction.DataAccess.EF;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        internal readonly IDataContext Database;
        internal readonly DbSet<Trade> dbSet;

        public TradeRepository(IDataContext context)
        {
            Database = context;
            dbSet = context.Set<Trade>();
        }

        public IQueryable<Trade> Entities => dbSet;

        public void AddTrade(Trade trade)
        {
            dbSet.Add(trade);
        }

        public void DeleteTrade(Trade trade)
        {
            if (Database.Entry(trade).State == EntityState.Detached)
                dbSet.Attach(trade);

            dbSet.Remove(trade);
        }

        public void DeleteTradeById(int id)
        {
            Trade trade = dbSet.Find(id);
            DeleteTrade(trade);
        }

        public IEnumerable<Trade> FindTrades(Expression<Func<Trade, bool>> filter = null,
            Func<IQueryable<Trade>, IOrderedQueryable<Trade>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Trade> query = dbSet;

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

        public Trade GetTradeById(int id)
        {
            return dbSet.Find(id);
        }

        public void UpdadeTrade(Trade trade)
        {
            dbSet.Attach(trade);
            Database.Entry(trade).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
