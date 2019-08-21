import { Component, OnInit } from '@angular/core';
import { Trade } from 'src/app/models/trade/trade';
import { TradeService } from 'src/app/services/trade.service';
import { Pagination } from 'src/app/models/pagination/pagination';

@Component({
  selector: 'app-trades-list',
  templateUrl: './trades-list.component.html',
  styleUrls: ['./trades-list.component.css']
})
export class TradesListComponent implements OnInit {
  trades: Trade[];
  pagination: Pagination;


  maxPrice: string;
  start: string;
  end: string;
  perPage: number;

  categoryId: number = 0;
  constructor(private tradeService: TradeService) { }

  ngOnInit() {
    this.tradeService.getTrades(10, 1, '', '', '', 0)
      .subscribe(
        (data) => {
          this.trades = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      );
  }

  onCategorySelected(id) {
    this.categoryId = id;
    this.tradeService.getTrades(this.perPage, 1, this.maxPrice, this.end, this.start, this.categoryId)
      .subscribe(
        (data) => {
          this.trades = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      )
  }

  applyFiltering() {
    this.tradeService.getTrades(this.perPage, 1, this.maxPrice, this.end, this.start, this.categoryId)
      .subscribe(
        (data) => {
          this.trades = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      )
  }

  nextPage() {
    this.tradeService.getTrades(this.perPage, this.pagination.CurrentPage + 1, this.maxPrice, this.end, this.start, this.categoryId)
      .subscribe(
        data => {
          this.trades = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      );
  }

  prevPage() {
    this.tradeService.getTrades(this.perPage, this.pagination.CurrentPage - 1, this.maxPrice, this.end, this.start, this.categoryId)
      .subscribe(
        data => {
          this.trades = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      );
  }

  resetFilter() {
    this.categoryId = 0;
    this.maxPrice = '';
    this.perPage = 10;
    this.end = '';
    this.start = '';

    this.onCategorySelected(this.categoryId);
  }

}
