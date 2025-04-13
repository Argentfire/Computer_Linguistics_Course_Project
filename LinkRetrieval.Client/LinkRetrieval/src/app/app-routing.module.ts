import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './@components/about/about.component';
import { HomeComponent } from './@components/home/home.component';
import { SearchFormComponent } from './@components/search-form/search-form.component';
import { SearchHistoryComponent } from './@components/search-history/search-history.component';
import { SearchResultViewComponent } from './@components/search-result-view/search-result-view.component';


const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'search-form', component: SearchFormComponent },
  { path: 'search-history', component: SearchHistoryComponent },
  { path: 'about', component: AboutComponent },
  { path: 'view-result', component: SearchResultViewComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
