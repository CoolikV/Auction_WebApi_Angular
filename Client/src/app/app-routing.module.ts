import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TradingLotListComponent } from './components/trading-lots/trading-lot-list/trading-lot-list.component';
import { TradesListComponent } from './components/trades/trades-list/trades-list.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { NewLotComponent } from './components/trading-lots/new-lot/new-lot.component';


const routes: Routes = [
  { path: "", component: TradesListComponent },
  { path: "trades", component: TradesListComponent },
  { path: "profile/lots", component: TradingLotListComponent },
  { path: "login", component: LoginComponent },
  { path: "register", component: RegisterComponent },
  { path: "newLot", component: NewLotComponent },
  { path: "/profile/trades", component: TradesListComponent },

  { path: "**", component: TradesListComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
