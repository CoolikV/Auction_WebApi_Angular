using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.BusinessLogic.DataTransfer
{
    public class TradingLotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Img { get; set; }
        public double Price { get; set; }
        public int TradeDuration { get; set; }
        public bool IsVerified { get; set; }
        public CategoryDTO Category { get; set; }
        public UserDTO User { get; set; }

        public TradingLotDTO()
        {
            IsVerified = false;
        }
    }
}
