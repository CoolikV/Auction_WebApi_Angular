﻿using Auction.BusinessLogic.DTOs.Trade;
using System;
using System.Collections.Generic;

namespace Auction.BusinessLogic.Interfaces
{
    public interface ITradeService : IDisposable
    {
        void StartTrade(int lotId);
        void RateTradingLot(RateDTO rate, string userName);
        TradeDTO GetTradeById(int id);
        TradeDTO GetTradeByLotId(int lotId);
        IEnumerable<TradeDTO> GetTradesForPage(int pageNum, int pageSize, DateTime? startDate,
            DateTime? endDate, double? maxBet, string lotName, out int pagesCount, out int totalItemsCount);
        IEnumerable<TradeDTO> GetUserTrades(string userId, int pageNum, int pageSize, string tradesState, DateTime? startDate,
            DateTime? endDate, double? maxBet, string lotName, out int pagesCount, out int totalItemsCount);
    }
}
