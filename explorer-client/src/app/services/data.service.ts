import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {ChartsResultDto, RubixCard} from '../models/rubixcardsdto';

@Injectable()
export class DataService {
  private readonly API_URL = 'https://explorer-api.azurewebsites.net/api/Explorer/';

 
  constructor(private httpClient: HttpClient) {}


  getCardsData(input:number) {
	return this.httpClient.get<RubixCard>(this.API_URL+"Cards?input="+input);
   }
   getTransactionsData(input:number) {
      return this.httpClient.get<ChartsResultDto[]>(this.API_URL+"DateWiseTransactions?input="+input);
  }
  getTokensData(input:number) {
    return this.httpClient.get(this.API_URL+"DateWiseTokens?input="+input);
  }
}




