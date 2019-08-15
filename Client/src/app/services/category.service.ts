import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { Category } from '../models/category/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  public routePrefix = '/api/categories'

  constructor(private httpService: HttpInternalService) { }

  public getCategoryById(id: number) {
    return this.httpService.getFullRequest<Category>(`${this.routePrefix}/${id}`)
  }
}
