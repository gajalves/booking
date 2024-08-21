import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApartmentModalComponent } from './apartment-modal.component';

describe('ApartmentModalComponent', () => {
  let component: ApartmentModalComponent;
  let fixture: ComponentFixture<ApartmentModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApartmentModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApartmentModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
