import { Component, OnInit, Input } from '@angular/core';
import { CategoryService } from 'src/app/services/category.service';
import { Category } from 'src/app/models/category/category';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-category-item',
  templateUrl: './category-item.component.html',
  styleUrls: ['./category-item.component.css']
})
export class CategoryItemComponent implements OnInit {
  @Input()
  category: Category;

  ngOnInit() {
  }
}
