import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ValoresEventoComponent } from './valores-evento.component';

describe('DetalhesEventoComponent', () => {
  let component: ValoresEventoComponent;
  let fixture: ComponentFixture<ValoresEventoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ValoresEventoComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ValoresEventoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
