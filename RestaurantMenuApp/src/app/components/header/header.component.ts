import { Component, OnInit, Renderer2 } from '@angular/core';

import { HeaderService } from "./header.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  
  language: any
  storeGuid: any;
  logoImage: any;
  colorCode: any;
  feedBackIcon: any;

  constructor(private _renderer: Renderer2,
    private _headerService: HeaderService) {
  }

  ngOnInit(): void {
    this.language = localStorage.getItem('lang') !== null ? localStorage.getItem('lang') : "EN"
    localStorage.setItem('lang', this.language);
    this._renderer.setAttribute(document.querySelector('html'), 'dir', this.language === 'AR' ? 'rtl' : 'ltr');

    this._headerService.currentStoreId.subscribe((res) => {
      this.storeGuid = res;
    })

    this._headerService.logo.subscribe((res) => {
      this.logoImage = res;
    })

    this._headerService.feedBackIcon.subscribe((res) => {
      this.feedBackIcon = res;
    })

    this._headerService.colorCode.subscribe((res) => {
      this.colorCode = res
      document.querySelector("body").style.cssText = "--color-code: " + this.colorCode;
    })
    
  }

  onLanguage(lang) {
    this.language = lang
    localStorage.setItem('lang', this.language);
    location.reload();
  }

  onBack() {
    this._headerService.backClick('Back button click');
  }
}
