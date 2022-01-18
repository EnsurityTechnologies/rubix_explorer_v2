import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { TransactionInfoDto } from 'src/app/models/rubixcardsdto';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-search-trans-info',
  templateUrl: './search-trans-info.component.html',
  styleUrls: ['./search-trans-info.component.css']
})
export class SearchTransInfoComponent implements OnInit {

  showError:boolean=false;
  transId: string = "";

  transInfo: TransactionInfoDto = new TransactionInfoDto(); 

  showNoContentBlock:boolean=false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,public dataService: DataService,private spinner: NgxSpinnerService
  ) {}

  ngOnInit() {
   
    const id = this.route.snapshot.paramMap.get('id')!;
    this.getTransInfo(id);
  }
 
  getTransInfo(id:any)
  {
    this.spinner.show();
    this.dataService.getTransactionInfo(id).subscribe((data:TransactionInfoDto) => 
    {
      if(data!=null){
        this.showNoContentBlock=false;
        this.transInfo.transaction_id = data.transaction_id;
        this.transInfo.sender_did = data.sender_did;
        this.transInfo.receiver_did = data.receiver_did;
        this.transInfo.token = data.token;
      }
      else{
        this.showNoContentBlock=true;
      }
   
      this.spinner.hide();
    });
  }


  //Send coin form validation
  searchform = new FormGroup({
    inputId: new FormControl('', [
      Validators.required]),
  });
  get f() {
    return this.searchform.controls;
  }

  onSubmit() {
    if(this.searchform.valid)
    {
      if(this.searchform.value.inputId.length==64)
      { 
          this.showError=false;
          this.getTransInfo(this.searchform.value.inputId);
          return true;
      }
      this.showError=true;
      return false;
    }
    return false;
  }
}
