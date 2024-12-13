import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfilePermissionsComponent } from './profile-permissions.component';

describe('ProfilePermissionsComponent', () => {
  let component: ProfilePermissionsComponent;
  let fixture: ComponentFixture<ProfilePermissionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfilePermissionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProfilePermissionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
