import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PsToolbarComponent } from './ps-toolbar.component';

describe('PsToolbarComponent', () => {
  let component: PsToolbarComponent;
  let fixture: ComponentFixture<PsToolbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PsToolbarComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PsToolbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
