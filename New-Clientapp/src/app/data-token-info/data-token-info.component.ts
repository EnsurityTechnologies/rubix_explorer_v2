import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataTokenInfoDto, NFTCreatorInputDto, TransactionInfoDto } from '../modals/rubixcardsdto';
import { DataService } from '../services/data.service';

export interface Creatorinfo {
  key: string;
  value: string;
}

@Component({
  selector: 'app-data-token-info',
  templateUrl: './data-token-info.component.html',
  styleUrls: ['./data-token-info.component.css']
})
 
export class DataTokenInfoComponent implements OnInit {

  tokenId: string = "";

  transpage = 1;
  transactionsList: any; 
  transItemsPerPage = 10;
  totalTransItems : any; 
  spinstatus:boolean=true;

  transactionInfo:boolean = false;
  nftInfo:boolean = false;
  
  nftColorInfo:boolean = false;

  quorum_listInfoObject:any = {};
  quorum_listInfoData:any = [];
  pledgerDID:string = '';
  quorum_list_insideTokensObjest:any = {};

  datatokensInfoObject:any = {};
  datatokensInfoData:any = [];


  datatokenInfo: DataTokenInfoDto = new DataTokenInfoDto(); 

  constructor(private route: ActivatedRoute,
    private router: Router,public dataService: DataService) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.dataService.getDataTokensInfo(id).subscribe((data:any) => 
    {
      this.datatokenInfo.transaction_id = data.transaction_id;
      this.datatokenInfo.commiter = data.commiter;
      this.datatokenInfo.amount = data.amount;
      this.datatokenInfo.creationTime = data.creation_time;
      this.datatokenInfo.volume = data.volume;
      this.quorum_listInfoObject = JSON.parse(data.quorum_list);
      this.datatokensInfoObject = JSON.parse(data.datatokens);
      
      // for (var type in this.quorum_listInfoObject) {
      //   var item1 = {quorumKey:'',quorumValue:''};
      //   item1.quorumKey = type;
      //   item1.quorumValue = this.quorum_listInfoObject[type];
      //   this.quorum_listInfoData.push(item1);
      // }
      for (var type in this.quorum_listInfoObject) {
        this.pledgerDID = type;
        this.quorum_list_insideTokensObjest = this.quorum_listInfoObject[type];
      }
      for (var type in this.quorum_list_insideTokensObjest) {
        var item1 = {quorumKey:'',quorumValue:0};
        item1.quorumKey = type;
        item1.quorumValue = this.quorum_list_insideTokensObjest[type];
        this.quorum_listInfoData.push(item1);
      }


      for (var type in this.datatokensInfoObject) {
        var item2 = {dataTokenKey:'',dataTokenValue:''};
        item2.dataTokenKey = type;
        item2.dataTokenValue = this.datatokensInfoObject[type];
        this.datatokensInfoData.push(item2);
      }

      this.spinstatus = false;
      // this.loadGrids(data.quorum_list);

      // console.log(this.nftCreatorinfo)

      // if(this.nftCreatorinfo.color == null)
      // {
      //   this.nftColorInfo = false;
      // }

      // if(data.transactionType == 0){
      //   this.transactionInfo = true;
      // }
      // else
      // {
      //   this.nftInfo = true;
      // }

    });
  }
  // loadGrids(tokenId:any) {

  //   this.dataService.getTransactionListInfoForTokenId(this.transpage, this.transItemsPerPage,tokenId).subscribe((data: any) => {

  //     this.transactionsList = data.items;
  //     this.totalTransItems = data.count;
  //     this.spinstatus = false
  //   });
  // }
  // detailTransFunction(transaction_id: any) {
  //   this.router.navigate(['/transinfo/' + transaction_id]);
  // }
  // userInfoFunction(userId: any) {
  //   this.router.navigate(['/userinfo/' + userId]);
  // }
}
