import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {

  private _listners = new Subject<any>();

  private storeId = new BehaviorSubject('');
  currentStoreId = this.storeId.asObservable();

  private logoImage = new BehaviorSubject('');
  logo = this.logoImage.asObservable();

  private feedBackIconImage = new BehaviorSubject('');
  feedBackIcon = this.feedBackIconImage.asObservable();

  private color = new BehaviorSubject('');
  colorCode = this.color.asObservable();

  updateStoreId(input: string) {
    this.storeId.next(input)
  }

  updateConceptTheme(logo: string, color: string, feedBackIcon:string) {
    this.logoImage.next(logo);
    this.color.next(color);
    this.feedBackIconImage.next(feedBackIcon);
  }

  listen(): Observable<any> {
    return this._listners.asObservable();
  }

  backClick(filterBy: string) {
    this._listners.next(filterBy);
  }
}
