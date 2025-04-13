import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchResultViewComponent } from './search-result-view.component';

describe('SearchResultViewComponent', () => {
  let component: SearchResultViewComponent;
  let fixture: ComponentFixture<SearchResultViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchResultViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchResultViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
