import { Component, OnInit, Input } from '@angular/core';
import { Trade } from 'src/app/models/trade/trade';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-trade-item',
  templateUrl: './trade-item.component.html',
  styleUrls: ['./trade-item.component.css']
})
export class TradeItemComponent implements OnInit {

  @Input()
  trade: Trade;
  staticFolder = environment.images;

  constructor() { }

  ngOnInit() {
  }

}
