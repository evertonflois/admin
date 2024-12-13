import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PsRowinfoComponent } from './ps-rowinfo.component';

describe('PsRowinfoComponent', () => {
  let component: PsRowinfoComponent;
  let fixture: ComponentFixture<PsRowinfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PsRowinfoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PsRowinfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
