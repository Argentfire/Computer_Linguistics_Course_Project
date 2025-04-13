import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { SearchRequest } from '../../models/searchRequest';


@Injectable({
  providedIn: 'root'
})

export class SearchResultService {
  private baseURL: string = "https://localhost:7009";
  constructor(
    private httpClient: HttpClient) {
  }



  getSearchResult<T>(searchResultId: string) {
    return this.httpClient.get(`${this.baseURL}/GetSearch/${searchResultId}`);
  }

  getSearchResults<T>() {
    return this.httpClient.get(`${this.baseURL}/GetSearches`);
  }
}
