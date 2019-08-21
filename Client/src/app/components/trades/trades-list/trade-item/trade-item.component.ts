import { Component, OnInit, Input } from '@angular/core';
import { Trade } from 'src/app/models/trade/trade';
import { environment } from 'src/environments/environment';
import { TradeService } from 'src/app/services/trade.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { Rate } from 'src/app/models/trade/rate';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-trade-item',
  templateUrl: './trade-item.component.html',
  styleUrls: ['./trade-item.component.css']
})
export class TradeItemComponent implements OnInit {

  @Input()
  trade: Trade;
  staticFolder = environment.images;
  betSum: number;

  rate = {} as Rate;

  isAuthneticated: boolean;
  constructor(private tradeService: TradeService, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.isAuthneticated = this.authService.isUserAuthenicated();

  }

  makeBet() {
    if (!this.isAuthneticated)
      this.router.navigate(['/login']);
    else if (this.trade.LastPrice < this.betSum) {
      this.rate.TradeId = this.trade.Id;
      this.rate.Sum = this.betSum;

      this.tradeService.rateTrade(this.rate)
        .subscribe(
          data => alert("Successfuly rated " + this.trade.TradingLot.Name),
          (err: HttpErrorResponse) => alert(err.error.Message)
        )
    }
    else {
      alert("Bet sum must be > " + this.trade.LastPrice)
    }
  }
}
