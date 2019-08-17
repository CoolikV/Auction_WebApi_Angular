import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { TradingLotListComponent } from './components/trading-lots/trading-lot-list/trading-lot-list.component';
import { TradesListComponent } from './components/trades/trades-list/trades-list.component';
import { TradingLotDetailComponent } from './components/trading-lots/trading-lot-detail/trading-lot-detail.component';
import { TradeDetailComponent } from './components/trades/trade-detail/trade-detail.component';


const routes: Routes = [
  { path: "", component: HomeComponent },
  { path: "trades", component: TradesListComponent },
  { path: "trades/:id", component: TradeDetailComponent },
  { path: "lots", component: TradingLotListComponent },
  { path: "lots/:id", component: TradingLotDetailComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
