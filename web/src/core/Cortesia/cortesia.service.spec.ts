import { TestBed } from '@angular/core/testing';

import { CortesiaService } from './cortesia.service';

describe('CortesiaService', () => {
  let service: CortesiaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CortesiaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
