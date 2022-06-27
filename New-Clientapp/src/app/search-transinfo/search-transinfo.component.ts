import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NFTCreatorInputDto, TransactionInfoDto } from '../modals/rubixcardsdto';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-search-transinfo',
  templateUrl: './search-transinfo.component.html',
  styleUrls: ['./search-transinfo.component.css']
})
export class SearchTransinfoComponent implements OnInit {

  tokenId: string = "";
  showError:boolean=false;
  transInfo: TransactionInfoDto = new TransactionInfoDto(); 

  nftCreatorinfo:NFTCreatorInputDto = new NFTCreatorInputDto();
  transactionInfo:boolean = false;
  nftInfo:boolean = false;

  nftCreatorInfoObject:any = {};
  nftCreatorInputData:any = [];


  showNoContentBlock:boolean=false;

  spinstatus:boolean=true;

  constructor(private route: ActivatedRoute,
    private router: Router,public dataService: DataService) { 
      const id = this.route.snapshot.paramMap.get('id')!;
    this.tokenId=id;
    }

  ngOnInit() {
    this.getTransInfo(this.tokenId)
  }
  getTransInfo(id:any)
  {
      this.dataService.getTransactionInfo(id).subscribe((data:TransactionInfoDto) => 
      {
        console.log(data);
        if(data!=null){
          this.showNoContentBlock=false;
          this.transInfo.transaction_id = data.transaction_id;
          this.transInfo.sender_did = data.sender_did;
          this.transInfo.receiver_did = data.receiver_did;
          this.transInfo.token = data.token;
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

          if(data.transactionType == 0){
            this.transactionInfo = true;
          }
          else
          {
            this.nftInfo = true;
          }

          console.log(this.nftCreatorinfo);
        }
        else{
          this.showNoContentBlock=true;
        }
        this.spinstatus=false;

      });
  }
  detailTransFunction(transaction_id: any) {
    this.router.navigate(['/transinfo/' + transaction_id]);
  }
  userInfoFunction(userId: any) {
    this.router.navigate(['/userinfo/' + userId]);
  }

}