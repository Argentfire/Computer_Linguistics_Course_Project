import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { SearchRequest } from '../../models/searchRequest';


@Injectable({
  providedIn: 'root'
})

export class SearchFormService {
  private baseURL: string = "https://localhost:7009";

  constructor(
    private httpClient: HttpClient) {
  }

  searchPageForURLs<T>(searchRequest: SearchRequest) {
    return this.httpClient.post<SearchRequest>(`${this.baseURL}/AddSearch`, searchRequest);
  }
}
