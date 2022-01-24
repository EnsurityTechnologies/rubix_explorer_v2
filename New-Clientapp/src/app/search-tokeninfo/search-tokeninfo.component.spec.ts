import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SearchTokeninfoComponent } from './search-tokeninfo.component';

describe('SearchTokeninfoComponent', () => {
  let component: SearchTokeninfoComponent;
  let fixture: ComponentFixture<SearchTokeninfoComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchTokeninfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchTokeninfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
