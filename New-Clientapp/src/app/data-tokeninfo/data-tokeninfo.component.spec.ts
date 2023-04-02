import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataTokeninfoComponent } from './data-tokeninfo.component';

describe('DataTokeninfoComponent', () => {
  let component: DataTokeninfoComponent;
  let fixture: ComponentFixture<DataTokeninfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataTokeninfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataTokeninfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
