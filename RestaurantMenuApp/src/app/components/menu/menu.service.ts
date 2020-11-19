import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable, Subject } from 'rxjs';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  rootURL:any;
  
  constructor(private http: HttpClient) {
    this.rootURL = environment.url + environment.api + "Menu/"
   }

  getMenu(code) {
    let lang = localStorage.getItem('lang');
    return this.http.get(this.rootURL + lang + "/" + code).map(res => res);
  }
}
