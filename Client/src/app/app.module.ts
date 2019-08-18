import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { CategoryItemComponent } from './components/categories/category-list/category-item/category-item.component';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './components/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { CategoryListComponent } from './components/categories/category-list/category-list.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TradeDetailComponent } from './components/trades/trade-detail/trade-detail.component';
import { TradingLotListComponent } from './components/trading-lots/trading-lot-list/trading-lot-list.component';
import { TradingLotItemComponent } from './components/trading-lots/trading-lot-list/trading-lot-item/trading-lot-item.component';
import { TradingLotDetailComponent } from './components/trading-lots/trading-lot-detail/trading-lot-detail.component';
import { AuthComponent } from './components/auth/auth.component';
import { TradesListComponent } from './components/trades/trades-list/trades-list.component';
import { TradeItemComponent } from './components/trades/trades-list/trade-item/trade-item.component';


@NgModule({
  declarations: [
    AppComponent,
    CategoryItemComponent,
    HomeComponent,
    NavbarComponent,
    CategoryListComponent,
    TradeDetailComponent,
    TradingLotListComponent,
    TradingLotItemComponent,
    TradingLotDetailComponent,
    TradesListComponent,
    TradeItemComponent,
    TradeDetailComponent,
    AuthComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
