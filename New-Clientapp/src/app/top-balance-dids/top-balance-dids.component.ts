import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from '../services/data.service';
import * as XLSX from 'xlsx';
@Component({
  selector: 'app-top-balance-dids',
  templateUrl: './top-balance-dids.component.html',
  styleUrls: ['./top-balance-dids.component.css']
})
export class TopBalanceDidsComponent implements OnInit {
  fileName= 'walletbalances.xlsx';
  wallets: any[] = [];
  spinstatus: boolean = true;
  interval: any;
  pageNumber = 1;
  pageSize = 10;
  loading = false;


  constructor(public httpClient: HttpClient,private router: Router,public dataService: DataService) { 
    
  }


  ngOnInit(): void {
    this.loadMore();
  }
  
  loadMore()
  {
    if (this.loading) {
      return; // Prevent multiple simultaneous requests
    }
    this.loading = true;
    this.spinstatus = true;
     this.dataService.getTopBalanceUserDIDs(this.pageNumber, this.pageSize).subscribe((data: any) => {

       this.wallets =  this.wallets.concat(data);
       this.pageNumber++;
       this.loading = false;
       this.spinstatus = false;
     });
   
  }
  userDidInfo(did: any) {
    this.router.navigate(['/userinfo/' + did]);
  }
  exportexcel(): void
  {
    /* pass here the table id */
    let element = document.getElementById('wallet-balances');
    const ws: XLSX.WorkSheet =XLSX.utils.table_to_sheet(element);
 
    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
 
    /* save to file */  
    XLSX.writeFile(wb, this.fileName);
 
  }
}
