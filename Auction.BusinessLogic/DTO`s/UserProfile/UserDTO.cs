using Auction.BusinessLogic.DTOs.Trade;
using Auction.BusinessLogic.DTOs.TradingLot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Auction.BusinessLogic.DTOs.UserProfile
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime BirthDate { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string Role { get; set; }

        [JsonIgnore]
        public ICollection<TradingLotDTO> TradingLots { get; set; }
        [JsonIgnore]
        public ICollection<TradeDTO> Trades { get; set; }

        public UserDTO()
        {
            TradingLots = new List<TradingLotDTO>();
            Trades = new List<TradeDTO>();
        }
    }
}
