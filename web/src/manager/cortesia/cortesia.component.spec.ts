import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CortesiaComponent } from './cortesia.component';

describe('CortesiaComponent', () => {
  let component: CortesiaComponent;
  let fixture: ComponentFixture<CortesiaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CortesiaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CortesiaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
