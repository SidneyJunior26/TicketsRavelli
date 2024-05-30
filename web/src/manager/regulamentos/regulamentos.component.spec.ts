import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegulamentosComponent } from './regulamentos.component';

describe('RegulamentosComponent', () => {
  let component: RegulamentosComponent;
  let fixture: ComponentFixture<RegulamentosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegulamentosComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegulamentosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
