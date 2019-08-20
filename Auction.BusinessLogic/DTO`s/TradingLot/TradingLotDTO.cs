using Auction.BusinessLogic.DTOs.Category;
using Auction.BusinessLogic.DTOs.UserProfile;
using Newtonsoft.Json;

namespace Auction.BusinessLogic.DTOs.TradingLot
{
    public class TradingLotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public double Price { get; set; }
        public int TradeDuration { get; set; }
        public int CategoryId { get; set; }
        public string Owner { get; set; }

        [JsonIgnore]
        public CategoryDTO Category { get; set; }

        [JsonIgnore]
        public UserDTO User { get; set; }
    }
}
