import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroInscricaoComponent } from './cadastro-inscricao.component';

describe('CadastroInscricaoComponent', () => {
  let component: CadastroInscricaoComponent;
  let fixture: ComponentFixture<CadastroInscricaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CadastroInscricaoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CadastroInscricaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
