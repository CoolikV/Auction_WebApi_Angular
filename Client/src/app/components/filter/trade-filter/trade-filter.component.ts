import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-trade-filter',
  templateUrl: './trade-filter.component.html',
  styleUrls: ['./trade-filter.component.css']
})
export class TradeFilterComponent implements OnInit {

  maxPrice: number;
  start: string;
  end: string;
  perPage: number;
  // @Output()
  // maxPrice = new EventEmitter<number>();


  constructor() { }

  ngOnInit() {
  }

}
