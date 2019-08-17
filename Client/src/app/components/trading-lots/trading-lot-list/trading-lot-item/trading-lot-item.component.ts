import { Component, OnInit, Input } from '@angular/core';
import { TradingLot } from 'src/app/models/trading-lot/trading-lot';

@Component({
  selector: 'app-trading-lot-item',
  templateUrl: './trading-lot-item.component.html',
  styleUrls: ['./trading-lot-item.component.css']
})
export class TradingLotItemComponent implements OnInit {
  @Input()
  lot: TradingLot;

  constructor() { }

  ngOnInit() {
  }

}
