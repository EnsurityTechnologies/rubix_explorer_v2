import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { TransInfoComponent } from './trans-info.component';

describe('TransInfoComponent', () => {
  let component: TransInfoComponent;
  let fixture: ComponentFixture<TransInfoComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ TransInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
