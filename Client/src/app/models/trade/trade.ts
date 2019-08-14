import { TradingLot } from '../trading-lot/trading-lot';

export interface Trade {
    id: number;
    tradingLot: TradingLot
    daysLeft: number;
    endsOn: Date;
    lastPrice: number;
}
