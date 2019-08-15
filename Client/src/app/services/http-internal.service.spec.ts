/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { HttpInternalService } from './http-internal.service';

describe('Service: HttpInternal', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HttpInternalService]
    });
  });

  it('should ...', inject([HttpInternalService], (service: HttpInternalService) => {
    expect(service).toBeTruthy();
  }));
});
