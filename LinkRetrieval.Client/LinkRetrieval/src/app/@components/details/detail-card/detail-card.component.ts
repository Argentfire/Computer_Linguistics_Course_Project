import { Component, ElementRef, input, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  standalone: false,
  selector: 'app-detail-card',
  templateUrl: './detail-card.component.html',
  styleUrl: './detail-card.component.scss'
})
export class DetailCardComponent implements OnInit {
  @Input() icon!: string;
  @Input() title!: string;
  @Input() content!: string;
  @Input() isNonInteractive!: boolean;
  @ViewChild('htmlViewDialog', { static: true })
  htmlViewDialogElement!: ElementRef<HTMLDialogElement>;

  constructor(
    private route: ActivatedRoute) {

  }

  ngOnInit(): void {

  }

  showHTMLContentModal() {
    // alert('This is a placeholder for showing HTML content in a modal.');
    if (this.htmlViewDialogElement) {
      const dialog: HTMLDialogElement = this.htmlViewDialogElement.nativeElement;
      dialog.showModal();
    }
  }
}
