import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Pagination } from 'src/app/models/pagination/pagination';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {
  @Input()
  pagination: Pagination;

  @Output()
  nextPage = new EventEmitter<number>();

  @Output()
  prevPage = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  toPrevPage() {
    this.prevPage.emit();
  }

  toNextPage() {
    this.nextPage.emit();
  }

}
