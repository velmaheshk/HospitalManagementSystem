import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Billitem } from './billitem';

describe('Billitem', () => {
  let component: Billitem;
  let fixture: ComponentFixture<Billitem>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Billitem]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Billitem);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
