import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { Trade } from '../models/trade/trade';
import { NewTradeDto } from '../models/trade/new-trade-dto';
import { RateDto } from '../models/trade/rate-dto';

@Injectable({
  providedIn: 'root'
})
export class TradeService {
  public routePrefix = "/api/trades/"

  constructor(private httpService: HttpInternalService) { }

  public getTradeById(id: number) {
    this.httpService.getFullRequest<Trade>(`${this.routePrefix}${id}`);
  }

  public getTrades(pageSize: number, pageNumber: number, endsOn: Date, startsOn: Date) {
    this.httpService.getFullRequest<Trade[]>(`${this.routePrefix}`, { pageSize, pageNumber, endsOn, startsOn });
  }

  public rateTrade(rate: RateDto) {
    this.httpService.putFullRequest(`${this.routePrefix}`, rate);
  }

  public startTrade(trade: NewTradeDto) {
    this.httpService.postFullRequest(`${this.routePrefix}`, trade);
  }
}
