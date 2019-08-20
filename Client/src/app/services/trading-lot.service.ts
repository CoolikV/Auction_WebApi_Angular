import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { TradingLot } from '../models/trading-lot/trading-lot';
import { Category } from '../models/category/category';
import { NewTradingLot } from '../models/trading-lot/new-trading-lot';

@Injectable({
  providedIn: 'root'
})
export class TradingLotService {

  public routePrefix = '/api/lots'
  constructor(private httpService: HttpInternalService) { }

  public getLotById(id: number) {
    return this.httpService.getFullRequest<TradingLot>(`${this.routePrefix}/${id}`);
  }

  public getLots(pageSize: number, pageNumber: number, categoryId: number, minPrice: number, maxPrice: number, lotName: string) {
    return this.httpService.getFullRequest<TradingLot[]>(`${this.routePrefix}`, { pageSize, pageNumber, categoryId, minPrice, maxPrice, lotName });
  }

  public getCategoryForLot(lotId: number) {
    return this.httpService.getRequest<Category>(`${this.routePrefix}/${lotId}/category`);
  }

  public createLot(lot: NewTradingLot) {
    return this.httpService.postFullRequest(`${this.routePrefix}`, lot);
  }

  public updateLot(id: number, lot: NewTradingLot) {
    return this.httpService.putFullRequest(`${this.routePrefix}/${id}`, lot);
  }

  public verifyLot(lotId: number) {
    return this.httpService.patchFullRequest(`${this.routePrefix}/${lotId}`)
  }

  public deleteLot(lotId: number) {
    return this.httpService.deleteFullRequest(`${this.routePrefix}/${lotId}`)
  }

}
