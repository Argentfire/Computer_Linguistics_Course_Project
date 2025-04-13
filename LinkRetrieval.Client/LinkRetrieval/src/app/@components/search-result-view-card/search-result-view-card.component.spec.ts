import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchResultViewCardComponent } from './search-result-view-card.component';

describe('SearchResultViewCardComponent', () => {
  let component: SearchResultViewCardComponent;
  let fixture: ComponentFixture<SearchResultViewCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchResultViewCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchResultViewCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
