import { Component, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/services/category.service';
import { Category } from 'src/app/models/category/category';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {

  constructor(private categoryService: CategoryService) { }
  public category = {} as Category;

  ngOnInit() {
    this.categoryService
      .getCategoryById(4)
      .subscribe(
        (response) => {
          this.category = response.body;
          console.log(this.category);
        }
      )
  }

}
