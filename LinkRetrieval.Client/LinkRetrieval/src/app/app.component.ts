import { Component } from '@angular/core';

@Component({
  standalone: false,
    selector: 'app-root',
  template: `
      <app-navigation/>
      <router-outlet/>
    `
  })

export class AppComponent {
  title = 'Пчелен Магазин Милкови';
}
