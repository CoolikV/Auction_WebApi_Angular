import { Component, OnInit } from '@angular/core';
import { Trade } from 'src/app/models/trade/trade';
import { TradeService } from 'src/app/services/trade.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-trades-list',
  templateUrl: './trades-list.component.html',
  styleUrls: ['./trades-list.component.css']
})
export class TradesListComponent implements OnInit {
  trades: Trade[];

  constructor(private tradeService: TradeService) { }

  ngOnInit() {
    this.tradeService.getTrades(10, 1, null, null)
      .subscribe(
        (data) => {
          this.trades = [...data.body];
        },
        (err: HttpErrorResponse) => {

        }
      )
  }

}
