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

        public Trade GetTradeById(int id)
        {
            try
            {
                return dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddTrade(Trade trade)
        {
            try
            {
                dbSet.Add(trade);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTrade(Trade trade)
        {
            try
            {
                if (Database.Entry(trade).State == EntityState.Detached)
                    dbSet.Attach(trade);

                dbSet.Remove(trade);
            }
            catch (Exception ex)
            {
                throw ex;
            }    
        }

        public void DeleteTradeById(int id)
        {
            try
            {
                Trade trade = dbSet.Find(id);
                DeleteTrade(trade);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Trade> FindTrades(Expression<Func<Trade, bool>> filter = null,
            Func<IQueryable<Trade>, IOrderedQueryable<Trade>> orderBy = null)
        {
            IQueryable<Trade> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
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

        public void UpdateTrade(Trade trade)
        {
            try
            {
                dbSet.Attach(trade);
                Database.Entry(trade).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
