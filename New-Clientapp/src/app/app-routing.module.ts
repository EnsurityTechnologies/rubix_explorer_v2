import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { DetailAddressComponent } from './detail-address/detail-address.component';
import { HomeComponent } from './home/home.component';
import { TokensComponent } from './tokens/tokens.component';
import { TransInfoComponent } from './trans-info/trans-info.component';
import { TransactionsComponent } from './transactions/transactions.component';
import { UserInfoComponent } from './user-info/user-info.component';


const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
