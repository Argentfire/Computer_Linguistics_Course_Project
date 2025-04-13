export class SearchResult {
  id: string;
  searchTime: string;
  url: string;
  regexPattern: string;

  constructor(id: string, searchTime: string, url: string, regexPattern: string) {
    this.id = id;
    this.searchTime = searchTime;
    this.url = url;
    this.regexPattern = regexPattern;
  }
}
