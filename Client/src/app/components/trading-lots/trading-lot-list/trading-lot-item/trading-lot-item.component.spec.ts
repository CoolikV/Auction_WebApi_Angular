/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TradingLotItemComponent } from './trading-lot-item.component';

describe('TradingLotItemComponent', () => {
  let component: TradingLotItemComponent;
  let fixture: ComponentFixture<TradingLotItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TradingLotItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TradingLotItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
