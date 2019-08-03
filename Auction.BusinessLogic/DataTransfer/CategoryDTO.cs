using System.Collections.Generic;

namespace Auction.BusinessLogic.DataTransfer
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TradingLotDTO> TradingLots { get; set; }

        public CategoryDTO()
        {
            TradingLots = new List<TradingLotDTO>();
        }
    }
}
