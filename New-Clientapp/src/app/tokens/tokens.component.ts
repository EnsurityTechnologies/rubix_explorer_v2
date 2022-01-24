import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-tokens',
  templateUrl: './tokens.component.html',
  styleUrls: ['./tokens.component.css']
})
export class TokensComponent implements OnInit {
 
  constructor( public httpClient: HttpClient,private router: Router,
    public dataService: DataService) { }

    tokenspage = 1;
    tokens: any; 
    tokensItemsPerPage = 20;
    totalTokensItems : any; 
    spinstatus:boolean=true;
    interval : any;

  ngOnInit() {
    this.loadGrids();
    this.interval = setInterval(()=>{ 
      this.loadGrids();
    },10000);
  }

  ngOnDestroy() {
    clearInterval(this.interval);
  }

  loadGrids()
  {
  
    this.dataService.getTokens(this.tokenspage,this.tokensItemsPerPage).subscribe((data: any) => {
      this.tokens =  data.items;
      this.totalTokensItems = data.count;
      this.spinstatus = false;
    });
   
  }
  detailTokenFunction(token_id:any)
  {

    this.router.navigate(['/searchtokentransinfo/'+token_id]);

  }
}
