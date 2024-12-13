import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PsSelectComponent } from './ps-select.component';

describe('PsSelectComponent', () => {
  let component: PsSelectComponent;
  let fixture: ComponentFixture<PsSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PsSelectComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PsSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
