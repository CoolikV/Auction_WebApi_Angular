using System;
using Auction.DataAccess.EF;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;

namespace Auction.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AuctionContext _context;
        private GenericRepository<TradingLot> tradingLotRepository;
        private GenericRepository<Trade> tradeRepository;
        private GenericRepository<Category> categoryRepository;

        public UnitOfWork(string connectionString)
        {
            _context = new AuctionContext(connectionString);
        }

        public IGenericRepository<TradingLot> TradingLots
        {
            get
            {
                if (tradingLotRepository == null)
                    tradingLotRepository = new GenericRepository<TradingLot>(_context);
                return tradingLotRepository;
            }
        }

        public IGenericRepository<Trade> Trades
        {
            get
            {
                if (tradeRepository == null)
                    tradeRepository = new GenericRepository<Trade>(_context);
                return tradeRepository;
            }
        }

        public IGenericRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new GenericRepository<Category>(_context);
                return categoryRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
