import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {

  constructor(public httpClient: HttpClient,private router: Router,
    public dataService: DataService) { }

    wallets:any;
    spinstatus:boolean=true;
    interval : any;

  ngOnInit(): void {
    this.loadGrids();
    this.interval = setInterval(()=>{ 
      this.loadGrids();
    },10000);
  }

  ngOnDestroy() {
    clearInterval(this.interval);
  }

  loadGrids()
  {
  
    this.dataService.getTopWallets().subscribe((data: any) => {
      this.wallets =  data.items;
      this.spinstatus = false;
    });
   
  }
  userDidInfo(did: any) {
    this.router.navigate(['/userinfo/' + did]);
  }
}
