export class SearchRequest {
  url!: string;
  regexPattern!: string;

  constructor(url: string, regexPattern: string) {
    this.url = url;
    this.regexPattern = regexPattern;
  }
}
