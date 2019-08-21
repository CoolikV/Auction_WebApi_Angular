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
        private AuctionContext _context;
        private ITradingLotRepository _tradingLotRepository;
        private ITradeRepository _tradeRepository;
        private ICategoryRepository _categoryRepository;
        private IUserProfileRepository _userRepository;
        
        private AppUserManager _userManager;
        private UserRoleManager _userRoleManager;

        public UnitOfWork(string connectionString)
        {
            _context = new AuctionContext(connectionString);
        }

        public IUserProfileRepository UserProfiles
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserProfileRepository(_context);
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
                    _userManager = new AppUserManager(new UserStore<AppUser>(_context));
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

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        #region IDisposable Support
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
