import { TestBed } from '@angular/core/testing';

import { MenuAppService } from './menu-app.service';

describe('MenuAppService', () => {
  let service: MenuAppService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MenuAppService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
