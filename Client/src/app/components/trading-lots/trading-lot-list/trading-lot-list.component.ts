import { Component, OnInit } from '@angular/core';
import { TradingLot } from 'src/app/models/trading-lot/trading-lot';
import { Pagination } from 'src/app/models/pagination/pagination';
import { UserProfileService } from 'src/app/services/user-profile.service';

@Component({
  selector: 'app-trading-lot-list',
  templateUrl: './trading-lot-list.component.html',
  styleUrls: ['./trading-lot-list.component.css']
})
export class TradingLotListComponent implements OnInit {

  maxPrice: string;
  minPrice: string;
  perPage: number;
  lotName: string;

  tradingLots: TradingLot[];
  pagination: Pagination;
  categoryId: number = 0;

  constructor(private userProfService: UserProfileService) { }

  ngOnInit() {
    this.userProfService.getUserLots(10, 1)
      .subscribe(
        (data) => {
          this.tradingLots = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      )
  }

  onCategorySelected(id) {
    this.categoryId = id;
    this.userProfService.getUserLots(this.perPage, 1, this.maxPrice, this.minPrice, this.categoryId)
      .subscribe(
        (data) => {
          this.tradingLots = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      )
  }

  applyFiltering() {
    console.log('max' + this.maxPrice + " " + "min " + this.minPrice)
    this.userProfService.getUserLots(this.perPage, 1, this.maxPrice, this.minPrice, this.categoryId, this.lotName)
      .subscribe(
        (data) => {
          console.log(data)
          this.tradingLots = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      )
  }

  nextPage() {
    this.userProfService.getUserLots(this.perPage, this.pagination.CurrentPage + 1, this.maxPrice, this.minPrice, this.categoryId, this.lotName)
      .subscribe(
        data => {
          this.tradingLots = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      );
  }

  prevPage() {
    this.userProfService.getUserLots(this.perPage, this.pagination.CurrentPage - 1, this.maxPrice, this.minPrice, this.categoryId, this.lotName)
      .subscribe(
        data => {
          this.tradingLots = [...data.body];
          this.pagination = JSON.parse(data.headers.get('Paging-Headers'));
        }
      );
  }

  resetFilter() {
    this.categoryId = 0;
    this.maxPrice = '';
    this.perPage = 10;
    this.maxPrice = '';
    this.lotName = '';
    this.onCategorySelected(this.categoryId);
  }
}
