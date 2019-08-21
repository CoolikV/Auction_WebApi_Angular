import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CategoryService } from 'src/app/services/category.service';
import { Category } from 'src/app/models/category/category';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  @Input()
  public categories: Category[];

  @Output()
  selectedCategory = new EventEmitter<number>();

  constructor(private categoryService: CategoryService) { }
  ngOnInit() {
    this.categoryService
      .getCategories()
      .subscribe(
        (resp) => {
          this.categories = [...resp.body];
        }
        , (err: HttpErrorResponse) => {
          console.log(err)
        }
      )
  }

  onChanged(id: any) {
    this.selectedCategory.emit(id)
  }

}
