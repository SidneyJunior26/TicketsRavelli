import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MensagemConfirmacaoComponent } from './mensagem-confirmacao.component';

describe('MensagemConfirmacaoComponent', () => {
  let component: MensagemConfirmacaoComponent;
  let fixture: ComponentFixture<MensagemConfirmacaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MensagemConfirmacaoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MensagemConfirmacaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
