import { Component, OnInit } from '@angular/core';
import { TradingLot } from 'src/app/models/trading-lot/trading-lot';
import { TradingLotService } from 'src/app/services/trading-lot.service';

@Component({
  selector: 'app-trading-lot-list',
  templateUrl: './trading-lot-list.component.html',
  styleUrls: ['./trading-lot-list.component.css']
})
export class TradingLotListComponent implements OnInit {

  tradingLots: TradingLot[];
  pagination: {};

  constructor(private tradingLotService: TradingLotService) { }

  ngOnInit() {
    this.tradingLotService
      .getLots(10, 1, 0, 101, 50000, "")
      .subscribe(
        (resp) => {
          this.tradingLots = [...resp.body];
          this.pagination = resp.headers.get('Paging-Headers');
          console.log(this.pagination);
          console.log(this.tradingLots);
          console.log(resp.url)
        }
      )
  }
}
