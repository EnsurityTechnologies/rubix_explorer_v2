import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { UserInfoDto } from '../models/rubixcardsdto';
import { DataService } from '../services/data.service';
@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {

 userInfo: UserInfoDto = new UserInfoDto(); 
  
  constructor(
    private route: ActivatedRoute,
    private router: Router, public dataService: DataService
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;
    console.log(id);
    this.dataService.getUserInfo(id).subscribe((data:UserInfoDto) => 
    {
      this.userInfo.balance = data.balance;
      this.userInfo.peerid = data.peerid;
      this.userInfo.ipaddress = data.ipaddress==""?"..":data.ipaddress;
      this.userInfo.user_did = data.user_did;
      console.log(this.userInfo);
    });
  }

}
