import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserInfoDto } from '../modals/rubixcardsdto';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {

  userInfo: UserInfoDto = new UserInfoDto(); 
  transactions: any;
  spinstatus:boolean=true;

  constructor(private route: ActivatedRoute,
    private router: Router, public dataService: DataService) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.dataService.getUserInfo(id).subscribe((data:UserInfoDto) => 
    {
      this.userInfo.peerid = data.peerid;
      this.userInfo.user_did = data.user_did;
      this.spinstatus = false
    });
    this.dataService.getTransactionsByDID(id).subscribe((resp:any)=>{
       console.log(resp);
       this.transactions=resp.items;
    });
  }

}
