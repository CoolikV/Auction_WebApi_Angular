using Auction.DataAccess.EF;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Auction.DataAccess.Repositories
{
    public class TradingLotRepository : ITradingLotRepository
    {
        internal readonly AuctionContext Database;
        internal readonly DbSet<TradingLot> dbSet;

        public TradingLotRepository(AuctionContext context)
        {
            Database = context;
            dbSet = context.Set<TradingLot>();
        }

        public TradingLot GetTradingLotById(int id)
        {
            try
            {
                return dbSet.Find(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void AddTradingLot(TradingLot tradingLot)
        {
            try
            {
                dbSet.Add(tradingLot);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTradingLot(TradingLot tradingLot)
        {
            try
            {
                dbSet.Attach(tradingLot);
                Database.Entry(tradingLot).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTradingLot(TradingLot tradingLot)
        {
            try
            {
                if (Database.Entry(tradingLot).State == EntityState.Detached)
                    dbSet.Attach(tradingLot);

                dbSet.Remove(tradingLot);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTradingLotById(int id)
        {
            try
            {
                TradingLot tradingLot = dbSet.Find(id);
                DeleteTradingLot(tradingLot);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

        public IQueryable<TradingLot> FindTradingLots(Expression<Func<TradingLot, bool>> filter = null)
        {
            IQueryable<TradingLot> query = dbSet;

            return filter == null ? query : query.Where(filter);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
