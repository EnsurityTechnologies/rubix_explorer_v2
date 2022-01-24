import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TransactionInfoDto } from '../modals/rubixcardsdto';
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
        if(data!=null){
          this.showNoContentBlock=false;
          this.transInfo.transaction_id = data.transaction_id;
          this.transInfo.sender_did = data.sender_did;
          this.transInfo.receiver_did = data.receiver_did;
          this.transInfo.token = data.token;
          this.transInfo.amount=data.amount;
          this.transInfo.creationTime=data.creationTime;
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