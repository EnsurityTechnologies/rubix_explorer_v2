import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { DataTokenInfoComponent } from './data-token-info.component';


describe('DataTokenInfoComponent', () => {
  let component: DataTokenInfoComponent;
  let fixture: ComponentFixture<DataTokenInfoComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ DataTokenInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataTokenInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
