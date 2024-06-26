import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventsManagerComponent } from './painel.component';

describe('EventsManagerComponent', () => {
  let component: EventsManagerComponent;
  let fixture: ComponentFixture<EventsManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EventsManagerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(EventsManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
