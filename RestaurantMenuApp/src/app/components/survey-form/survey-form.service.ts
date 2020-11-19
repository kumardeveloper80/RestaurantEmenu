import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SurveyFormService {
  rootURL: any;
  headers = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(private http: HttpClient) {
    this.rootURL = environment.url + environment.api + "Survey/"
  }

  getSurveyForm(storeid) {
    return this.http.get(this.rootURL + storeid).map(res => res);
  }

  saveSurveyForm(surveyForm) {
    return this.http.post(this.rootURL, surveyForm).map(res => res);
  }
}
