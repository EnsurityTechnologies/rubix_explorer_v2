import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DataService } from './services/data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'RubixExplorer';
  status: boolean = false;
  showError:boolean=false;
  spinstatus:boolean=false;
  // public placeholder : string = "Token or Transaction Hash";

  constructor(public httpClient: HttpClient,private router: Router,
    public dataService: DataService) {}

    ngOnInit() {
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
      if(this.searchform.value.inputId.length==67)
      {
        this.showError=false;
        this.router.navigate(['/searchtokentransinfo/'+this.searchform.value.inputId]);
        this.searchform.reset();
        // this.placeholder = "Token Hash";
        return true;
      }
      else if(this.searchform.value.inputId.length==64)
      {
        this.showError=false;
        this.router.navigate(['/searchtransinfo/'+this.searchform.value.inputId]);
        this.searchform.value.inputId="";
        this.searchform.reset();
        // this.placeholder = "Transaction Hash";
        return true;
      }
      else if (this.searchform.value.inputId.length == 46 || this.searchform.value.inputId.length == 59) {
        this.showError = false;
        this.router.navigate(['/userinfo/' + this.searchform.value.inputId]);
        return true;
      }
      this.showError = true;
      this.searchform.reset();
      return false;
      
    }
    this.searchform.reset();
    return false;
  }
  toggleMenu(){
    this.status = !this.status; 
  }

}
