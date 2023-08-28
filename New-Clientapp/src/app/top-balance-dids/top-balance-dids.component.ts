import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-top-balance-dids',
  templateUrl: './top-balance-dids.component.html',
  styleUrls: ['./top-balance-dids.component.css']
})
export class TopBalanceDidsComponent implements OnInit {

  wallets: any[] = [];
  spinstatus: boolean = true;
  interval: any;
  currentPage = 1;
  itemsPerPage = 10;
  displayedWallets: any[];

  constructor(public httpClient: HttpClient,private router: Router,public dataService: DataService) { 
    
  }

  onPageChange(pageNumber: number) {
    this.currentPage = pageNumber;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.displayedWallets = this.wallets.slice(startIndex, endIndex);
  }
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
  
    this.dataService.getTopBalanceUserDIDs().subscribe((data: any) => {
      this.wallets =  data;
      this.displayedWallets = this.wallets.slice(0, this.itemsPerPage);
      this.spinstatus = false;
    });
   
  }
  userDidInfo(did: any) {
    this.router.navigate(['/userinfo/' + did]);
  }
  getPages(): number[] {
    const pageCount = Math.ceil(this.wallets.length / this.itemsPerPage);
    const visiblePageCount = 5; // Number of dynamic page numbers to display
    const currentPageIndex = this.currentPage - 1;

    if (pageCount <= visiblePageCount) {
      return Array(pageCount).fill(0).map((_, index) => index + 1);
    } else {
      const startPage = Math.max(currentPageIndex - Math.floor(visiblePageCount / 2), 0);
      const endPage = Math.min(startPage + visiblePageCount - 1, pageCount - 1);
      const dynamicPages = Array(endPage - startPage + 1).fill(0).map((_, index) => startPage + index + 1);

      // Include first and last pages only if they are not in the dynamic set
      let pagesToShow: number[] = [];
      if (!dynamicPages.includes(1)) {
        pagesToShow.push(1);
      }
      pagesToShow = pagesToShow.concat(dynamicPages);
      if (!dynamicPages.includes(pageCount)) {
        pagesToShow.push(pageCount);
      }

      return pagesToShow;
    }
  }
}
