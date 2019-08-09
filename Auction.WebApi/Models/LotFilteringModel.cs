namespace Auction.WebApi.Models
{
    public class LotFilteringModel
    {
        private int? _categoryId;
        private double? _minPrice = 0;
        private double? _maxPrice;
        public string LotName { get; set; }

        public int? CategoryId
        {
            get
            {
                return _categoryId.Value;
            }
            set
            {
                _categoryId = value;
            }
        }

        public double? MaxPrice
        {
            get
            {
                return _maxPrice ?? null;
            }
            set
            {
                if (value < 0)
                    _maxPrice = null;
                else
                    _maxPrice = value;
            }
        }

        public double? MinPrice
        {
            get
            {
                return _minPrice ?? null;
            }
            set
            {
                if (value < 0)
                    _minPrice = 0;
                else
                    _minPrice = value;
            }
        }
    }
}