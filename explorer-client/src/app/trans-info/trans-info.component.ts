import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TransactionInfoDto } from '../models/rubixcardsdto';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-trans-info',
  templateUrl: './trans-info.component.html',
  styleUrls: ['./trans-info.component.css']
})
export class TransInfoComponent implements OnInit {

  transInfo: TransactionInfoDto = new TransactionInfoDto(); 
  constructor(
    private route: ActivatedRoute,
    private router: Router,public dataService: DataService
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.dataService.getTransactionInfo(id).subscribe((data:TransactionInfoDto) => 
    {
      console.log(data);
      this.transInfo.Id = data.Id;
      this.transInfo.transaction_id = data.transaction_id;
      this.transInfo.sender_did = data.sender_did;
      this.transInfo.receiver_did = data.receiver_did;
      this.transInfo.token = data.token;
      console.log(this.transInfo);
    });
  }
}
