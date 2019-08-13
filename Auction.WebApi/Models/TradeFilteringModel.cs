using System;

namespace Auction.WebApi.Models
{
    public class TradeFilteringModel : LotFilteringModel
    {
        private DateTime? _startDate;
        private DateTime? _endDate;

        public DateTime? EndsOn
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
        public DateTime? StartsOn
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