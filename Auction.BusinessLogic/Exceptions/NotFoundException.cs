using System;

namespace Auction.BusinessLogic.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Items not found.")
        { }

        public NotFoundException(string message) : base(message + " does not exist.")
        { }
    }
}
