import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SenhaPrimeiroAcessoComponent } from './senha-primeiro-acesso.component';

describe('SenhaPrimeiroAcessoComponent', () => {
  let component: SenhaPrimeiroAcessoComponent;
  let fixture: ComponentFixture<SenhaPrimeiroAcessoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SenhaPrimeiroAcessoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SenhaPrimeiroAcessoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
