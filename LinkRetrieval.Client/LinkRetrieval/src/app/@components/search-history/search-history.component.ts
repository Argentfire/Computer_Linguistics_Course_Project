import { Component, OnInit, ViewChild } from '@angular/core';
import { formatDate } from '@angular/common';
import { MatTable } from '@angular/material/table';
import { SearchResult } from '../../models/searchResult';
import { SearchResultService } from './search-history.service';
import { Router } from '@angular/router';

@Component({
  standalone: false,
  selector: 'app-search-history',
  templateUrl: './search-history.component.html',
  styleUrl: './search-history.component.scss'
})
export class SearchHistoryComponent implements OnInit {
  searchResults: SearchResult[] = [];
  displayedColumns: string[] = ['searchTime', 'url', 'regexPattern'];
  dataSource = [...this.searchResults];
  @ViewChild(MatTable) table!: MatTable<any>;
  constructor(private searchHistoryService: SearchResultService,
    private router: Router) {

  }

  ngOnInit(): void {
    this.searchHistoryService.getSearchResults().subscribe((data) => {
      this.searchResults = data as SearchResult[];
      for (let item of this.searchResults) {
        item.searchTime = formatDate(item.searchTime, 'yyyy-MM-dd HH:mm:ss', 'en-US');
      }
      this.dataSource = [...this.searchResults];
    });;
  }

  foo(sender: any) {
    this.router.navigate(['/view-result'], {
      queryParams: {
        searchResultId: sender.id
      }
    });
  }
}
