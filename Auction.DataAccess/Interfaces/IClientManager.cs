using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.DataAccess.Entities;

namespace Auction.DataAccess.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(User user);
    }
}
