import { Component, OnInit, Input } from '@angular/core';
import { TradingLot } from 'src/app/models/trading-lot/trading-lot';
import { TradeService } from 'src/app/services/trade.service';
import { NewTradeDto } from 'src/app/models/trade/new-trade-dto';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-trading-lot-item',
  templateUrl: './trading-lot-item.component.html',
  styleUrls: ['./trading-lot-item.component.css']
})
export class TradingLotItemComponent implements OnInit {
  @Input()
  lot: TradingLot;

  newTrade = {} as NewTradeDto;
  duration: number;
  constructor(private tradeService: TradeService) { }

  ngOnInit() {
  }

  startTrade() {
    this.newTrade.lotId = this.lot.Id;
    this.newTrade.tradeDuration = this.duration;

    console.log(this.newTrade);
    this.tradeService.startTrade(this.newTrade)
      .subscribe(
        (resp) => alert(`Trade for lot started`),
        (error: HttpErrorResponse) => alert(error.error.Message)
      )
  }

  isInputValid() {
    return this.duration && (this.duration > 0 && this.duration < 31);
  }
}
