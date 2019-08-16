import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { Category } from '../models/category/category';
import { NewCategoryDto } from '../models/category/new-category-dto';
import { TradingLot } from '../models/trading-lot/trading-lot';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  public routePrefix = '/api/categories'

  constructor(private httpService: HttpInternalService) { }

  public getCategoryById(id: number) {
    return this.httpService.getFullRequest<Category>(`${this.routePrefix}/${id}`);
  }

  public getCategories() {
    return this.httpService.getFullRequest<Category[]>(`${this.routePrefix}`);
  }

  public getLotsFromCategory(id: number, pageSize: number, pageNumber: number, minPrice: number, maxPrice: number, lotName: string) {
    this.httpService.getFullRequest<TradingLot[]>(`${this.routePrefix}/${id}/lots`, { pageSize, pageNumber, minPrice, maxPrice, lotName });
  }

  public getLotFromCategory(id: number, lotId: number) {
    this.httpService.getFullRequest<TradingLot>(`${this.routePrefix}/${id}/lots`, { lotId })
  }

  public createCategory(category: NewCategoryDto) {
    this.httpService.postFullRequest(`${this.routePrefix}`, category);
  }

  public changeCategoryName(id: number, newName: string) {
    this.httpService.putFullRequest(`${this.routePrefix}/${id}`, <NewCategoryDto>{ name: newName })
  }

  public deleteCategory(id: number) {
    this.httpService.deleteFullRequest(`${this.routePrefix}/${id}`);
  }
}
