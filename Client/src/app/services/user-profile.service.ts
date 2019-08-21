import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { TradingLot } from '../models/trading-lot/trading-lot';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  public routePrefix = '/api/profiles'

  constructor(private httpService: HttpInternalService) { }


  public getUserLots(pageSize: number, pageNumber: number, maxPrice?: string, minPrice?: string, categoryId?: number, lotName?: string) {
    let userClaims = JSON.parse(localStorage.getItem('userClaims'));

    let params: HttpParams = new HttpParams();

    if (maxPrice)
      params = params.set('maxPrice', maxPrice);
    if (minPrice)
      params = params.set('minPrice', minPrice);
    if (lotName)
      params = params.set('lotName', lotName);
    if (categoryId)
      params = params.set('categoryId', "" + categoryId);
    if (!pageSize || pageSize > 20) {
      params = params.set('pageSize', '20')
    }
    else {
      params = params.set('pageSize', "" + pageSize)
    }
    params = params.set('pageNumber', "" + pageNumber)

    console.log(params.toString());
    return this.httpService.getFullRequest<TradingLot[]>(`${this.routePrefix}` + "/" + `${userClaims.Id}` + "/lots", params);
  }
}
