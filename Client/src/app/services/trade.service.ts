import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { Trade } from '../models/trade/trade';
import { NewTradeDto } from '../models/trade/new-trade-dto';
import { RateDto } from '../models/trade/rate-dto';

@Injectable({
  providedIn: 'root'
})
export class TradeService {
  public routePrefix = "/api/trades"

  constructor(private httpService: HttpInternalService) { }

  public getTradeById(id: number) {
    return this.httpService.getFullRequest<Trade>(`${this.routePrefix}/${id}`);
  }

  public getTrades(pageSize: number, pageNumber: number, maxPrice: string, endsOn?: string, startsOn?: string, categoryId?: number) {
    if (!maxPrice)
      maxPrice = '';
    if (!endsOn)
      endsOn = '';
    if (!startsOn)
      startsOn = '';
    if (!pageSize || pageSize > 20)
      pageSize = 20;
    return this.httpService.getFullRequest<Trade[]>(`${this.routePrefix}`, { pageSize, pageNumber, endsOn, startsOn, categoryId, maxPrice });
  }

  public rateTrade(rate: RateDto) {
    return this.httpService.putFullRequest(`${this.routePrefix}`, rate);
  }

  public startTrade(trade: NewTradeDto) {
    return this.httpService.postFullRequest(`${this.routePrefix}`, trade);
  }
}
