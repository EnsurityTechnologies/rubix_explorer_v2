import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SearchTransinfoComponent } from './search-transinfo.component';

describe('SearchTransinfoComponent', () => {
  let component: SearchTransinfoComponent;
  let fixture: ComponentFixture<SearchTransinfoComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchTransinfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchTransinfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
