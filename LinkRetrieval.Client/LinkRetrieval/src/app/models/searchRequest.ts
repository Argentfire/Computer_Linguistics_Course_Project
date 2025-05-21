export class SearchRequest {
  url!: string;
  regexPattern!: string;
  searchDepth!: number;

  constructor(url: string, regexPattern: string, searchDepth: number = 1) {
    this.url = url;
    this.regexPattern = regexPattern;
    this.searchDepth = searchDepth;
  }
}
