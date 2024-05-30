import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormasPagamentosComponent } from './formas-pagamentos.component';

describe('FormasPagamentosComponent', () => {
  let component: FormasPagamentosComponent;
  let fixture: ComponentFixture<FormasPagamentosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FormasPagamentosComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormasPagamentosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
