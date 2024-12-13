import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PsTextboxComponent } from './ps-textbox.component';

describe('PsTextboxComponent', () => {
  let component: PsTextboxComponent;
  let fixture: ComponentFixture<PsTextboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PsTextboxComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PsTextboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
