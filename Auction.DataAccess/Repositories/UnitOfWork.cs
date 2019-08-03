using Auction.DataAccess.EF;
using Auction.DataAccess.Identity.Entities;
using Auction.DataAccess.Identity.Repositories;
using Auction.DataAccess.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace Auction.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDataContext _context;
        private ITradingLotRepository _tradingLotRepository;
        private ITradeRepository _tradeRepository;
        private ICategoryRepository _categoryRepository;
        private IUserRepository _userRepository;
        
        private AppUserManager _userManager;
        private UserRoleManager _userRoleManager;

        public UnitOfWork(string connectionString)
        {
            _context = new AuctionContext(connectionString);
        }

        public IUserRepository Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public ITradingLotRepository TradingLots
        {
            get
            {
                if (_tradingLotRepository == null)
                    _tradingLotRepository = new TradingLotRepository(_context);
                return _tradingLotRepository;
            }
        }

        public ITradeRepository Trades
        {
            get
            {
                if (_tradeRepository == null)
                    _tradeRepository = new TradeRepository(_context);
                return _tradeRepository;
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_context);
                return _categoryRepository;
            }
        }

        public AppUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = new AppUserManager(new UserStore<AppUser>(_context as AuctionContext));
                return _userManager;
            }
        }

        public UserRoleManager UserRoleManager
        {
            get
            {
                if (_userRoleManager == null)
                    _userRoleManager = new UserRoleManager(new RoleStore<AppRole>(_context as AuctionContext));
                return _userRoleManager;
            }
        }

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();

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
