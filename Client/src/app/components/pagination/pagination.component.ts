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
  nextPage = new EventEmitter();

  @Output()
  prevPage = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  toPrevPage() {
    console.log(this.pagination);

    this.prevPage.emit();
  }

  toNextPage() {
    console.log(this.pagination);

    this.nextPage.emit();
  }

}
