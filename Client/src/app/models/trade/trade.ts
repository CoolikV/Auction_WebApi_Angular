import { TradingLot } from '../trading-lot/trading-lot';

export interface Trade {
    Id: number;
    TradingLot: TradingLot
    DaysLeft: number;
    EndsOn: Date;
    LastPrice: number;
}
