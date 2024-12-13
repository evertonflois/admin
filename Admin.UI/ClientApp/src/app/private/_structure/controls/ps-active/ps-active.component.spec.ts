import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PsActiveComponent } from './ps-active.component';

describe('PsActiveComponent', () => {
  let component: PsActiveComponent;
  let fixture: ComponentFixture<PsActiveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PsActiveComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PsActiveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
