import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HighchartsChartModule } from 'highcharts-angular';
import {HttpClientModule} from '@angular/common/http';
import { DataService } from './services/data.service';
import {NgxPaginationModule} from 'ngx-pagination';
import { UserInfoComponent } from './user-info/user-info.component';
import { TransInfoComponent } from './trans-info/trans-info.component';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CountUpModule } from 'ngx-countup';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from "ngx-spinner";
import { SearchTransInfoComponent } from './home/search-trans-info/search-trans-info.component';
import { SearchTokenInfoComponent } from './home/search-token-info/search-token-info.component';

const appRoutes: Routes = [
  { path: '', component: HomeComponent, },
  { path: 'trans-info/:id', component: TransInfoComponent },
  { path: 'user-info/:id', component: UserInfoComponent },
  { path: 'search-trans-info/:id', component: SearchTransInfoComponent },
  { path: 'search-token-info/:id', component: SearchTokenInfoComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    UserInfoComponent,
    TransInfoComponent,
    HomeComponent,
    SearchTransInfoComponent,
    SearchTokenInfoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HighchartsChartModule,
    HttpClientModule,
    NgxPaginationModule,
    CountUpModule,
    BrowserAnimationsModule,
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,

    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    )
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
