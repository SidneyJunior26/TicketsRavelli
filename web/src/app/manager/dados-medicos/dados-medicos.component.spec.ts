import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DadosMedicosComponent } from './dados-medicos.component';

describe('DadosMedicosComponent', () => {
  let component: DadosMedicosComponent;
  let fixture: ComponentFixture<DadosMedicosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DadosMedicosComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DadosMedicosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
