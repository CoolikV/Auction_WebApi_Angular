using System;

namespace Auction.BusinessLogic.Exceptions
{
    /// <summary>
    /// Exception class for database errors
    /// </summary>
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) { }
        public DatabaseException() : base() { }
    }
}
