import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchTransInfoComponent } from './search-trans-info.component';

describe('SearchTransInfoComponent', () => {
  let component: SearchTransInfoComponent;
  let fixture: ComponentFixture<SearchTransInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchTransInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchTransInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
