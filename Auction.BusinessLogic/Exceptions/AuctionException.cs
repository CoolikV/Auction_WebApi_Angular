using System;

namespace Auction.BusinessLogic.Exceptions
{
    /// <summary>
    /// Exception class for auction restrictions
    /// </summary>
    public class AuctionException : Exception
    {
        public AuctionException(string message) : base(message) { }

        public AuctionException() : base() { }
    }
}
