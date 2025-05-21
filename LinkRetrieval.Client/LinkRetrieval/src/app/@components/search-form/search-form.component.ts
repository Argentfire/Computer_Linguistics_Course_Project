import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { SearchRequest } from '../../models/searchRequest';
import { SearchFormService } from './search-form.service';

@Component({
  standalone: false,
  selector: 'app-search-form',
  templateUrl: './search-form.component.html',
  styleUrl: './search-form.component.scss',
})
export class SearchFormComponent {
  public urlAddress: string;
  public regexPattern: string;
  public searchDepth: number;
  public warningAccepted: boolean;

  resultVisible: boolean = false;

  constructor(private searchService: SearchFormService) {
    this.urlAddress = 'https://arenabg.com';
    this.regexPattern = `\\b((https?:\\/\\/|www\\.)[^\\s<>\"']+)`;
    this.searchDepth = 1;
    this.warningAccepted = false;
  }

  sendSearchRequest(): void {
    const searchRequest = new SearchRequest(this.urlAddress, this.regexPattern, this.searchDepth);
    this.searchService.searchPageForURLs(searchRequest).subscribe((data) => {
    });
  }

  setDepthLevel(event: any): void {
    if (this.searchDepth < 1) {
      this.searchDepth = 1;
    }
  }
}
