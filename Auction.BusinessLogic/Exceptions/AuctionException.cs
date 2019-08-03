using System;

namespace Auction.BusinessLogic.Exceptions
{
    public class AuctionException : Exception
    {
        public AuctionException(string message) : base(message) { }

        public AuctionException() : base() { }
    }
}
