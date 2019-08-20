import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Category } from 'src/app/models/category/category';

@Component({
  selector: 'app-category-item',
  templateUrl: './category-item.component.html',
  styleUrls: ['./category-item.component.css']
})
export class CategoryItemComponent implements OnInit {
  @Input()
  category: Category;

  @Output()
  onCategorySelected = new EventEmitter<number>();

  ngOnInit() {
  }

  selected(id) {
    this.onCategorySelected.emit(id);
  }
}
