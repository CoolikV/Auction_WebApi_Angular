using Auction.BusinessLogic.DTOs.TradingLot;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Auction.BusinessLogic.DTOs.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<TradingLotDTO> TradingLots { get; set; }

        public CategoryDTO()
        {
            TradingLots = new List<TradingLotDTO>();
        }
    }
}
