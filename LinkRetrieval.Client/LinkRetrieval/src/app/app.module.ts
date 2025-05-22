import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; // Import FormsModule
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AngularMaterialModule } from './@modules/angular-material.module';
import { CommonModule } from '@angular/common';
import { NavigationComponent } from './@components/navigation/navigation.component';
import { SearchFormComponent } from './@components/search-form/search-form.component';
import { SearchHistoryComponent } from './@components/search-history/search-history.component';
import { SearchResultViewComponent } from './@components/search-result-view/search-result-view.component';
import { DetailCardComponent } from './@components/details/detail-card/detail-card.component';

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    NavigationComponent,
    SearchFormComponent,
    SearchHistoryComponent,
    SearchResultViewComponent,
    DetailCardComponent
  ],
  imports: [
    AppRoutingModule,
    AngularMaterialModule,
    CommonModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
  ],
  providers: [],
})
export class AppModule {}
