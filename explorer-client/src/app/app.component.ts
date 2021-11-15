import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import * as Highcharts from 'highcharts';
import { DataService } from './services/data.service';
import {map} from 'rxjs/operators';
import {ActivityFilter, ChartsResultDto, RubixCard} from './models/rubixcardsdto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(public httpClient: HttpClient,
    public dataService: DataService) {}

    public d = new Date();

    public n = this.d.getUTCDate();

    rubixPrice:number= 0
    rubixUsersCount:number= 0
    tokensCount:number= 0
    transactionsCount:number= 0

    page = 1;
    transactions: any; 
    itemsPerPage = 10;
    totalItems : any; 


    ngOnInit() {
      this.loadcards(1);
      this.loadGrids();
    }

  transCharthighcharts = Highcharts;
  tokensCharthighcharts = Highcharts;
  usersCharthighcharts = Highcharts;

  public transoptions: any;
  public tokenoptions: any;


  changeDuriation(e:any) {
    this.loadcards(e.target.value)
  }

  loadcards(value:ActivityFilter)
  {
    this.dataService.getCardsData(value).subscribe(resp=>{
      this.rubixPrice=resp.rubixPrice;
      this.rubixUsersCount=resp.rubixUsersCount;
      this.tokensCount=resp.tokensCount;
      this.transactionsCount=resp.transactionsCount;
    });

   this.dataService.getTransactionsData(value).subscribe((response: any)=>{

    var keysArray:any = [];
    var valuesArray:any=[];
    response.forEach(function (item:any) {
      keysArray.push(item.key);
      valuesArray.push(item.value);
    }); 

       this.transoptions={
        title: {
          text: "Transactions:"+ ActivityFilter[value],
        },
        xAxis: {
          title: {
            text: ActivityFilter[value].toString()
          },
          categories:keysArray
        },
        yAxis: {
          title: {
            text: "Transactions"
          }
        },
        credits: {
          enabled: false
        },
        series: [{
          name: 'Count',
          data: valuesArray,
          type: 'column',
          showInLegend: false,
        }]
      };
      
       Highcharts.chart('transactionChart', this.transoptions);
     });

   this.dataService.getTokensData(value).subscribe(((response: any)=>{
     

    var keysArray:any = [];
    var valuesArray:any=[];
    response.forEach(function (item:any) {
      keysArray.push(item.key);
      valuesArray.push(item.value);
    }); 


    this.tokenoptions={
      title: {
        text: "Tokens:"+ ActivityFilter[value].toString(),
      },
      xAxis: {
        title: {
          text:  ActivityFilter[value]
        },
        categories:keysArray
      },
      yAxis: {
        title: {
          text: "Tokens"
        }
      },
      credits: {
        enabled: false
      },
      series: [{
        name: 'Count',
        data: valuesArray,
        type: 'column',
        showInLegend: false,
      }]
    }

    Highcharts.chart('tokenChart', this.tokenoptions);
   }));
  }

    loadGrids()
    {
      this.dataService.getTransactions(this.page,this.itemsPerPage).subscribe((data: any) => {
        console.log(data)
        this.transactions =  data.items;
        this.totalItems = data.count;
      });
    }
    gty(page: any){
      this.dataService.getTransactions(page,this.itemsPerPage).subscribe((data: any) => {
        console.log(data)
        this.transactions =  data.items;
        this.totalItems = data.count;
      });
    }
    detailTransFunction(transaction_id:any)
    {
     alert(transaction_id);
    }
    userInfoFunction(userId:any)
    {
      alert(userId);
    }
}
