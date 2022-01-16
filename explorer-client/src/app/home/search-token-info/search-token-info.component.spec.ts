import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchTokenInfoComponent } from './search-token-info.component';

describe('SearchTokenInfoComponent', () => {
  let component: SearchTokenInfoComponent;
  let fixture: ComponentFixture<SearchTokenInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchTokenInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchTokenInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
