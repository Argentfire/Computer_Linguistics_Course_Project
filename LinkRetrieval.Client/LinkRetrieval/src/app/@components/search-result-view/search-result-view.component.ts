import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SearchResultService } from '../search-history/search-history.service';

@Component({
  standalone: false,
  selector: 'app-search-result-view',
  templateUrl: './search-result-view.component.html',
  styleUrl: './search-result-view.component.scss',
})
export class SearchResultViewComponent implements OnInit {
  @Input() searchResultId: any;
  searchResult: any = null;

  constructor(
    private route: ActivatedRoute,
    private searchHistoryService: SearchResultService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.searchResultId = params['searchResultId'];
      this.searchHistoryService
        .getSearchResult(this.searchResultId)
        .subscribe((data) => {
          this.searchResult = data;
        });
    });
  }
}
