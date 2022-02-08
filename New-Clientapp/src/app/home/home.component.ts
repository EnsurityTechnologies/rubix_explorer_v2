import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivityFilter } from '../modals/rubixcardsdto';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(public httpClient: HttpClient, private router: Router,
    public dataService: DataService) { }

  public d = new Date();
  status: boolean = true;
  public n = this.d.getUTCDate();
  spinstatus:boolean=true;

  rubixPrice: number = 0
  rubixUsersCount: number = 0
  tokensCount: number = 0
  transactionsCount: number = 0
  curculatingSupplyCount: number = 0

  labelstring: String = 'Transactions';
  transpage = 1;
  transactions: any;
  transItemsPerPage = 20;
  totalTransItems: any;

  tokenspage = 1;
  tokens: any;
  tokensItemsPerPage = 20;
  totalTokensItems: any;
  interval : any;

  apiLink:string= "https://api.rubix.network/api/Explorer/Cards?input=1";

  ngOnInit() {
    this.loadcards(1);
    this.interval = setInterval(()=>{ 
      this.loadcards(1);
    },40000);
  }
  
  ngOnDestroy() {
    clearInterval(this.interval);
  }

  onClickTokens(event: Event) {
    this.status = false;
    this.labelstring = "Tokens"
  }

  onClickTransactions(event: Event) {
    this.status = true;
    this.labelstring = "Transactions"
  }

  public transoptions: any;
  public tokenoptions: any;

  showError: boolean = false;
  changeDuriation(e: any) {
    this.loadcards(e.target.value)
  }

  loadcards(value: ActivityFilter) {
    this.dataService.getCardsData(value).subscribe(resp => {
      this.rubixPrice=resp.rubixPrice;
      this.rubixUsersCount = resp.rubixUsersCount;
      this.tokensCount = resp.tokensCount;
      this.transactionsCount = resp.transactionsCount;
      this.curculatingSupplyCount = resp.curculatingSupplyCount;
      console.log(resp);
      this.spinstatus = false
    });
  }

  loadGrids() {
    this.dataService.getTransactions(this.transpage, this.transItemsPerPage).subscribe((data: any) => {
      this.transactions = data.items;
      this.totalTransItems = data.count;
    });

    this.dataService.getTokens(this.tokenspage, this.tokensItemsPerPage).subscribe((data: any) => {
      this.tokens = data.items;
      this.totalTokensItems = data.count;
    });
  }

  gtransEvent(transpage: any) {
    this.dataService.getTransactions(transpage, this.transItemsPerPage).subscribe((data: any) => {

      this.transactions = data.items;
      this.totalTransItems = data.count;
    });
  }

  gTokensEvent(tokenspage: any) {
    this.dataService.getTokens(tokenspage, this.tokensItemsPerPage).subscribe((data: any) => {

      this.tokens = data.items;
      this.totalTokensItems = data.count;
    });
  }

  detailTransFunction(transaction_id: any) {
    this.router.navigate(['/trans-info/' + transaction_id]);
  }
  
  userInfoFunction(userId: any) {
    this.router.navigate(['/user-info/' + userId]);
  }

  //Send coin form validation
  searchform = new FormGroup({
    inputId: new FormControl('', [
      Validators.required]),
  });
  get f() {
    return this.searchform.controls;
  }

  onSubmit() {
    if (this.searchform.valid) {
      if (this.searchform.value.inputId.length == 67) {
        this.showError = false;
        this.router.navigate(['/search-token-info/' + this.searchform.value.inputId]);
        return true;
      }
      else if (this.searchform.value.inputId.length == 64) {
        this.showError = false;
        this.router.navigate(['/search-trans-info/' + this.searchform.value.inputId]);
        return true;
      }
      this.showError = true;
      return false;
    }
    return false;
  }

  copytoClipBoard(val: string) {

    let selBox = document.createElement('textarea');

    selBox.style.position = 'fixed';

    selBox.style.left = '0';

    selBox.style.top = '0';

    selBox.style.opacity = '0';

    selBox.value = val;

    document.body.appendChild(selBox);

    selBox.focus();

    selBox.select();

    document.execCommand('copy');

    document.body.removeChild(selBox);

  }

  detailTokenFunction(token_id: any) {

    this.router.navigate(['/search-token-info/' + token_id]);

  }

  copyDID(val: string)

  {
  

    let selBox = document.createElement('textarea');

    selBox.style.position = 'fixed';

    selBox.style.left = '0';

    selBox.style.top = '0';

    selBox.style.opacity = '0';

    selBox.value = val;

    document.body.appendChild(selBox);

    selBox.focus();

    selBox.select();

    document.execCommand('copy');

    document.body.removeChild(selBox);

    // this.toastr.toastrConfig.positionClass = 'toast-center-center';

    // this.toastr.success("Id Copied successfully !!");

  }

}
