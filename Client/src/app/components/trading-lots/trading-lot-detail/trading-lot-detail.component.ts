import { Component, OnInit } from '@angular/core';
import { TradingLot } from 'src/app/models/trading-lot/trading-lot';
import { ActivatedRoute, Router, ParamMap, Params } from '@angular/router';
import { TradingLotService } from 'src/app/services/trading-lot.service';

@Component({
  selector: 'app-trading-lot-detail',
  templateUrl: './trading-lot-detail.component.html',
  styleUrls: ['./trading-lot-detail.component.css']
})
export class TradingLotDetailComponent implements OnInit {

  lot: TradingLot;
  lotId: number;

  constructor(
    private route: ActivatedRoute,
    private lotService: TradingLotService
  ) { }

  ngOnInit() {
    this.route
      .params
      .subscribe((params: Params) => {
        this.lotId = params['id'];
      });

    this.lotService
      .getLotById(this.lotId)
      .subscribe(
        (resp) => {
          this.lot = <TradingLot>resp.body;
          console.log(resp.body);
        }
      )
  }
}
