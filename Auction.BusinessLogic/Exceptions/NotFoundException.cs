using System;

namespace Auction.BusinessLogic.Exceptions
{
    /// <summary>
    /// Exception class for operation that returns empty search results
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Items not found.")
        { }

        public NotFoundException(string message) : base(message)
        { }
    }
}
