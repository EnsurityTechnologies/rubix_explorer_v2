import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { DetailAddressComponent } from './detail-address/detail-address.component';
import { TransactionsComponent } from './transactions/transactions.component';
import { TokensComponent } from './tokens/tokens.component';
import { CountUpModule } from 'ngx-countup';
import { DataService } from './services/data.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { TransInfoComponent } from './trans-info/trans-info.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { RouterModule, Routes } from '@angular/router';
import { SearchTransinfoComponent } from './search-transinfo/search-transinfo.component';
import { SearchTokeninfoComponent } from './search-tokeninfo/search-tokeninfo.component';
import { WalletComponent } from './wallet/wallet.component';


const appRoutes: Routes = [
  {path:'',component:HomeComponent},
  {path:'transactions',component:TransactionsComponent},
  {path:'tokens',component:TokensComponent},
  {path:'detailAddress',component:DetailAddressComponent},
  {path:'transinfo/:id',component:TransInfoComponent},
  {path:'userinfo/:id',component:UserInfoComponent},
  {path:'searchtransinfo/:id',component:SearchTransinfoComponent},
  {path:'searchtokentransinfo/:id',component:SearchTokeninfoComponent},
  {path:'wallets',component:WalletComponent},
];

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    DetailAddressComponent,
    TransactionsComponent,
    TokensComponent,
    TransInfoComponent,
    UserInfoComponent,
    SearchTransinfoComponent,
    SearchTokeninfoComponent,
    WalletComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CountUpModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true, relativeLinkResolution: 'legacy' } // <-- debugging purposes only
 // <-- debugging purposes only
    )
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
