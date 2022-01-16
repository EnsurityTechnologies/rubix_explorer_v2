import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { TransactionInfoDto } from 'src/app/models/rubixcardsdto';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-search-token-info',
  templateUrl: './search-token-info.component.html',
  styleUrls: ['./search-token-info.component.css']
})
export class SearchTokenInfoComponent implements OnInit {

  tokenId: string = "";
  showNoContentBlock:boolean=false;

  transpage = 1;
  transactionsList: any; 
  transItemsPerPage = 10;
  totalTransItems : any; 

  constructor(
    private route: ActivatedRoute,
    private router: Router,public dataService: DataService,private spinner: NgxSpinnerService
  ) {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.tokenId=id;
  }

  ngOnInit() {
    this.loadGrids(this.tokenId)
  }
  loadGrids(tokenId:any) {
    this.spinner.show();
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
      this.loadGrids(this.searchform.value.inputId);
      return true;
    }
    return false;
  }
}
