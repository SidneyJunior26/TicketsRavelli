import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EfetivarInscricaoComponent } from './efetivar-inscricao.component';

describe('EfetivarInscricaoComponent', () => {
  let component: EfetivarInscricaoComponent;
  let fixture: ComponentFixture<EfetivarInscricaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EfetivarInscricaoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EfetivarInscricaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
