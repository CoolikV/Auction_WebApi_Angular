using System;

namespace Auction.BusinessLogic.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) { }
        public DatabaseException() : base() { }
    }
}
