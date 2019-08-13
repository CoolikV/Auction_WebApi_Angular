namespace Auction.BusinessLogic.DataTransfer
{
    public class TradingLotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public double Price { get; set; }
        public int TradeDuration { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
        public UserDTO User { get; set; }
    }
}
