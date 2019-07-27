using System.Collections.Generic;

namespace Auction.BusinessLogic.DataTransfer
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<TradingLotDTO> TradingLots { get; set; }
        public ICollection<TradeDTO> Trades { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public UserDTO()
        {
            TradingLots = new List<TradingLotDTO>();
            Trades = new List<TradeDTO>();
        }
    }
}
