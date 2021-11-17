import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {ChartsResultDto, RubixCard, TransactionInfoDto, UserInfoDto} from '../models/rubixcardsdto';

@Injectable()
export class DataService {
  private readonly API_URL = 'https://api.rubix.network/api/Explorer/';

 
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

  getTransactions(page:any,size:any)
  {
    return this.httpClient.get(this.API_URL+"LatestTransactions?page="+page+"&pageSize="+size+"");
  }
  getTokens(page:any,size:any)
  {
    return this.httpClient.get(this.API_URL+"LatestTokens?page="+page+"&pageSize="+size+"");
  }
  getUserInfo(transactionid:any)
  {
    return this.httpClient.get<UserInfoDto>(this.API_URL+"userInfo/"+transactionid);
  }
  getTransactionInfo(transactionid:any)
  {
    return this.httpClient.get<TransactionInfoDto>(this.API_URL+"transactionInfo/"+transactionid);
  }
  getTransactionListInfoForTokenId(page:any,size:any,tokenId:any)
  {
    return this.httpClient.get(this.API_URL+"transactionListInfoForTokenId/?page="+page+"&pageSize="+size+""+"&token_Id="+tokenId);
  }
}




