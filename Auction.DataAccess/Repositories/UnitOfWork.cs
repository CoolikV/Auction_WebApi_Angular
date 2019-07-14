using System;
using System.Threading.Tasks;
using Auction.DataAccess.EF;
using Auction.DataAccess.Entities;
using Auction.DataAccess.Interfaces;
using Auction.DataAccess.Identity.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using Auction.DataAccess.Identity.Entities;

namespace Auction.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AuctionContext _context;
        private GenericRepository<TradingLot> _tradingLotRepository;
        private GenericRepository<Trade> _tradeRepository;
        private GenericRepository<Category> _categoryRepository;
        private GenericRepository<User> _userRepository;

        private UserManager _userManager;
        private UserRoleManager _userRoleManager;
        private IClientManager _clientManager;

        public UnitOfWork(string connectionString)
        {
            _context = new AuctionContext(connectionString);
        }

        public IGenericRepository<User> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new GenericRepository<User>(_context);
                return _userRepository;
            }
        }

        public IGenericRepository<TradingLot> TradingLots
        {
            get
            {
                if (_tradingLotRepository == null)
                    _tradingLotRepository = new GenericRepository<TradingLot>(_context);
                return _tradingLotRepository;
            }
        }

        public IGenericRepository<Trade> Trades
        {
            get
            {
                if (_tradeRepository == null)
                    _tradeRepository = new GenericRepository<Trade>(_context);
                return _tradeRepository;
            }
        }

        public IGenericRepository<Category> Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new GenericRepository<Category>(_context);
                return _categoryRepository;
            }
        }

        public UserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = new UserManager(new UserStore<AppUser>(_context));
                return _userManager;
            }
        }

        public UserRoleManager UserRoleManager
        {
            get
            {
                if (_userRoleManager == null)
                    _userRoleManager = new UserRoleManager(new RoleStore<AppRole>(_context));
                return _userRoleManager;
            }
        }

        public IClientManager ClientManager
        {
            get
            {
                if (_clientManager == null)
                    _clientManager = new ClientManager(_context);
                return _clientManager;
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
