using System;

namespace Auction.WebApi.Models
{
    public class TradeFilteringModel : LotFilteringModel
    {
        private DateTime? _startDate;
        private DateTime? _endDate;

        public DateTime? TradeEnds
        {
            get
            {
                return _endDate ?? null;
            }
            set
            {
                _endDate = value;
            }
        }
        public DateTime? TradeStarts
        {
            get
            {
                return _startDate ?? null;
            }
            set
            {
                _startDate = value;
            }
        }
    }
}