import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TransactionInfoDto } from '../models/rubixcardsdto';
import { DataService } from '../services/data.service';
import { NgxSpinnerService } from "ngx-spinner";
@Component({
  selector: 'app-trans-info',
  templateUrl: './trans-info.component.html',
  styleUrls: ['./trans-info.component.css']
})
export class TransInfoComponent implements OnInit {
  tokenId: string = "";

  transpage = 1;
  transactionsList: any; 
  transItemsPerPage = 10;
  totalTransItems : any; 

  transInfo: TransactionInfoDto = new TransactionInfoDto(); 
  constructor(
    private route: ActivatedRoute,
    private router: Router,public dataService: DataService,private spinner: NgxSpinnerService
  ) {}

  ngOnInit() {
    this.spinner.show();
    const id = this.route.snapshot.paramMap.get('id')!;
    this.dataService.getTransactionInfo(id).subscribe((data:TransactionInfoDto) => 
    {
      this.transInfo.transaction_id = data.transaction_id;
      this.transInfo.sender_did = data.sender_did;
      this.transInfo.receiver_did = data.receiver_did;
      this.transInfo.token = data.token;
      this.tokenId=data.token;
      this.loadGrids(this.tokenId);
    });
  }
  loadGrids(tokenId:any) {

    this.dataService.getTransactionListInfoForTokenId(this.transpage, this.transItemsPerPage,tokenId).subscribe((data: any) => {

      this.transactionsList = data.items;
      this.totalTransItems = data.count;
      this.spinner.hide();
    });
  }
  gtransListEvent(transpage: any) {
    this.spinner.show();
    this.dataService.getTransactionListInfoForTokenId(transpage, this.transItemsPerPage,this.tokenId).subscribe((data: any) => {

      this.transactionsList = data.items;
      this.totalTransItems = data.count;
      this.spinner.hide();
    });
  }
}
