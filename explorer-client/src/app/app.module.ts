import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

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
const appRoutes: Routes = [
  { path: '', component: HomeComponent, },
  { path: 'trans-info', component: TransInfoComponent },
  { path: 'user-info', component: UserInfoComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    UserInfoComponent,
    TransInfoComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HighchartsChartModule,
    HttpClientModule,
    NgxPaginationModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    )
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
