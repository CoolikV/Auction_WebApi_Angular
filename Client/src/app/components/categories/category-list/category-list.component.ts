import { Component, OnInit, Input } from '@angular/core';
import { CategoryService } from 'src/app/services/category.service';
import { Category } from 'src/app/models/category/category';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  @Input()
  public categories: Category[];

  constructor(private categoryService: CategoryService) { }

  ngOnInit() {
    this.categoryService
      .getCategories()
      .subscribe(
        (resp) => {
          this.categories = [...resp.body];
          console.log(this.categories);
        }
      )
  }
}
