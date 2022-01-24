import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent implements OnInit {

  constructor(public httpClient: HttpClient, private router: Router,
    public dataService: DataService) { }

  labelstring: String = 'Transactions';
  transpage = 1;
  transactions: any;
  transItemsPerPage = 20;
  totalTransItems: any;
  spinstatus:boolean=true;
  interval : any;


  ngOnInit() {
    this.loadGrids();
    this.interval = setInterval(()=>{ 
      this.loadGrids();
    },10000);
  }
  
  ngOnDestroy() {
    clearInterval(this.interval);
  }

  loadGrids() {
    
    this.dataService.getTransactions(this.transpage, this.transItemsPerPage).subscribe((data: any) => {
      this.transactions = data.items;
      this.totalTransItems = data.count;
      this.spinstatus = false; 
    });
  }

  detailTransFunction(transaction_id: any) {
    this.router.navigate(['/transinfo/' + transaction_id]);
  }
  userInfoFunction(userId: any) {
    this.router.navigate(['/userinfo/' + userId]);
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
