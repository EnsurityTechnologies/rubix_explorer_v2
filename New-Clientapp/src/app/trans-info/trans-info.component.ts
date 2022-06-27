import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NFTCreatorInputDto, TransactionInfoDto } from '../modals/rubixcardsdto';
import { DataService } from '../services/data.service';

export interface Creatorinfo {
  key: string;
  value: string;
}

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
  spinstatus:boolean=true;

  transactionInfo:boolean = false;
  nftInfo:boolean = false;
  
  nftColorInfo:boolean = false;

  nftCreatorinfo:NFTCreatorInputDto = new NFTCreatorInputDto();

  nftCreatorInfoObject:any = {};
  nftCreatorInputData:any = [];


  transInfo: TransactionInfoDto = new TransactionInfoDto(); 

  constructor(private route: ActivatedRoute,
    private router: Router,public dataService: DataService) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.dataService.getTransactionInfo(id).subscribe((data:TransactionInfoDto) => 
    {

      this.transInfo.transaction_id = data.transaction_id;
      this.transInfo.sender_did = data.sender_did;
      this.transInfo.receiver_did = data.receiver_did;
      this.transInfo.token = data.token;
      this.tokenId=data.token;
      this.transInfo.amount=data.amount;
      this.transInfo.creationTime=data.creationTime;
      this.transInfo.transactionType=data.transactionType;
      this.transInfo.nftToken=data.nftToken;
      this.transInfo.nftBuyer=data.nftBuyer;
      this.transInfo.nftSeller=data.nftSeller;
      this.transInfo.totalSupply=data.totalSupply;
      this.transInfo.editionNumber=data.editionNumber;
      this.transInfo.rbtTransactionId=data.rbtTransactionId;
      this.transInfo.userHash=data.userHash;
      this.nftCreatorInfoObject = JSON.parse(data.nftCreatorInput);
      
      for (var type in this.nftCreatorInfoObject) {
        var item = {key:'',value:''};
        item.key = type;
        item.value = this.nftCreatorInfoObject[type];
        this.nftCreatorInputData.push(item);
        }

      this.loadGrids(data.nftCreatorInput);

      // console.log(this.nftCreatorinfo)

      // if(this.nftCreatorinfo.color == null)
      // {
      //   this.nftColorInfo = false;
      // }

      if(data.transactionType == 0){
        this.transactionInfo = true;
      }
      else
      {
        this.nftInfo = true;
      }

    });
  }
  loadGrids(tokenId:any) {

    this.dataService.getTransactionListInfoForTokenId(this.transpage, this.transItemsPerPage,tokenId).subscribe((data: any) => {

      this.transactionsList = data.items;
      this.totalTransItems = data.count;
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
