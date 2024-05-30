import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AfiliadosEventoComponent } from './afiliados-evento.component';

describe('AfiliadosEventoComponent', () => {
  let component: AfiliadosEventoComponent;
  let fixture: ComponentFixture<AfiliadosEventoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AfiliadosEventoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AfiliadosEventoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
