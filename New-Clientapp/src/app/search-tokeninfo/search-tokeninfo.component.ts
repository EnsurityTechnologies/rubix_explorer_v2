import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TransactionInfoDto } from '../modals/rubixcardsdto';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-search-tokeninfo',
  templateUrl: './search-tokeninfo.component.html',
  styleUrls: ['./search-tokeninfo.component.css']
})
export class SearchTokeninfoComponent implements OnInit {

  showError:boolean=false;

  transInfo: TransactionInfoDto = new TransactionInfoDto(); 

  showNoContentBlock:boolean=false;
  tokenId: string = "";
  transpage = 1;
  transactionsList: any; 
  transItemsPerPage = 10;
  totalTransItems : any; 

  spinstatus:boolean=true;


  constructor(private route: ActivatedRoute,
    private router: Router,public dataService: DataService) { 
      const id = this.route.snapshot.paramMap.get('id')!;
    this.tokenId=id;
    }

  ngOnInit() {
    this.loadGrids(this.tokenId)
  }

  loadGrids(tokenId:any) {
    this.dataService.getTransactionListInfoForTokenId(this.transpage, this.transItemsPerPage,tokenId).subscribe((data: any) => {

      if(data.items.length>0)
      {
        this.showNoContentBlock=false;
        this.transactionsList = data.items;
        this.totalTransItems = data.count;
      }
      else
      {
        this.showNoContentBlock=true;
      }
      this.spinstatus = false
    });
  }
  
  detailTransFunction(transaction_id: any) {
    this.router.navigate(['/transinfo/' + transaction_id]);
  }
  userInfoFunction(userId: any) {
    this.router.navigate(['/userinfo/' + userId]);
  }

}
