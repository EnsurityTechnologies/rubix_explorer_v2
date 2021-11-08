import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {RubixCard} from '../models/rubixcardsdto';

@Injectable()
export class DataService {
  private readonly API_URL = 'https://localhost:44331/api/Explorer/';

 
  constructor(private httpClient: HttpClient) {}


  getCardsData(input:number) {
	return this.httpClient.get<RubixCard>(this.API_URL+"Cards?input="+input);
}
}




