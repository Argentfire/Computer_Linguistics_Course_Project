import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MatchResultsContainerComponent } from './match-results-container.component';

describe('MatchResultsContainerComponent', () => {
  let component: MatchResultsContainerComponent;
  let fixture: ComponentFixture<MatchResultsContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatchResultsContainerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MatchResultsContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
