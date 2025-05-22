export class MatchResult {
  id!: string;
  matchedText!: string;
  startIndex!: number;
  length!: number;
  searchId!: string;

  constructor(id: string, matchedText: string,
    startIndex: number, length: number, searchId: string) {
    this.id = id;
    this.matchedText = matchedText;
    this.startIndex = startIndex;
    this.length = length;
    this.searchId = searchId;
  }
}
