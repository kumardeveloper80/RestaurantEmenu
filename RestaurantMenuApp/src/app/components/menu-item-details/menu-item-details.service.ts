import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MenuItemDetailsService {
  rootURL:any;

  constructor(private http: HttpClient) { 
    this.rootURL = environment.url + environment.api + "Menu/"
  }

  getMenuItemDetails(code, id) {
    let lang = localStorage.getItem('lang');
    return this.http.get(this.rootURL + lang + "/" + code + "/" + id).map(res => res);;
  }
} 
