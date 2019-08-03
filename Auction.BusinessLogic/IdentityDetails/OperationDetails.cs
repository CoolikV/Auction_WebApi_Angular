namespace Auction.BusinessLogic.IdentityDetails
{
    public class OperationDetails
    {
        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }

        public OperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }
    }
}
