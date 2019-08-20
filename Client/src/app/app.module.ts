import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';

import { CategoryItemComponent } from './components/categories/category-list/category-item/category-item.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HomeComponent } from './components/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { CategoryListComponent } from './components/categories/category-list/category-list.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TradeDetailComponent } from './components/trades/trade-detail/trade-detail.component';
import { TradingLotListComponent } from './components/trading-lots/trading-lot-list/trading-lot-list.component';
import { TradingLotItemComponent } from './components/trading-lots/trading-lot-list/trading-lot-item/trading-lot-item.component';
import { TradingLotDetailComponent } from './components/trading-lots/trading-lot-detail/trading-lot-detail.component';
import { TradesListComponent } from './components/trades/trades-list/trades-list.component';
import { TradeItemComponent } from './components/trades/trades-list/trade-item/trade-item.component';
import { LotFilterComponent } from './components/filter/lot-filter/lot-filter.component';
import { TradeFilterComponent } from './components/filter/trade-filter/trade-filter.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BearerInterceptor } from './helpers/bearer.interceptor';
import { DatePipe } from '@angular/common';


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
    LotFilterComponent,
    TradeFilterComponent,
    PaginationComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    NgbModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BearerInterceptor,
      multi: true
    },
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
