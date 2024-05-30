import { TestBed } from '@angular/core/testing';

import { EventosService } from './core/Eventos/events.service';

describe('EventsService', () => {
  let service: EventosService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EventosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
