import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InscricoesAtletaComponent } from './inscricoes-atleta.component';

describe('InscricoesAtletaComponent', () => {
  let component: InscricoesAtletaComponent;
  let fixture: ComponentFixture<InscricoesAtletaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InscricoesAtletaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InscricoesAtletaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
