import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopBalanceDidsComponent } from './top-balance-dids.component';

describe('TopBalanceDidsComponent', () => {
  let component: TopBalanceDidsComponent;
  let fixture: ComponentFixture<TopBalanceDidsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TopBalanceDidsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TopBalanceDidsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
