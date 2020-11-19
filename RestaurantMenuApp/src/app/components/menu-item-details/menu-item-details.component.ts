import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuItemDetailsService } from '../menu-item-details/menu-item-details.service';
import { HeaderService } from "../header/header.service";

@Component({
  selector: 'app-menu-item-details',
  templateUrl: './menu-item-details.component.html',
  styleUrls: ['./menu-item-details.component.css']
})
export class MenuItemDetailsComponent implements OnInit {
  lang = localStorage.getItem('lang');
  menuItem: any;
  code: any;
  id: any;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _menuItemDetailsService: MenuItemDetailsService,
    private _headerService: HeaderService) {

    this._headerService.listen().subscribe((m: any) => {
      this.onBack();
    })
  }

  ngOnInit(): void {
    (<HTMLInputElement>document.getElementById('backBtnLi')).classList.remove("d-none");
    (<HTMLInputElement>document.getElementById('feedbackBtn')).classList.add("d-none");
    this.getMenuItemDetails();
  }

  getMenuItemDetails() {
    this._headerService.updateStoreId(sessionStorage.getItem("storeGuid"))
    this._headerService.updateConceptTheme(sessionStorage.getItem("logo"), sessionStorage.getItem("colorCode"), sessionStorage.getItem("feedBackIcon"));
    this.code = this._route.snapshot.paramMap.get('code');
    this.id = this._route.snapshot.paramMap.get('id');
    this._menuItemDetailsService.getMenuItemDetails(this.code, this.id).subscribe((res) => {
      if (res["status"] === 1) {
        this.menuItem = res["dataenum"];
      } else {
        alert(res["message"])
      }
    })
  }

  onBack() {
    this._router.navigateByUrl('/Emenu/' + this.code);
  }

  closeme()
  {
   document.getElementById("divoverlay").classList.add("close-mobile");
  }

  removeclass()
  {
    document.getElementById("divoverlay").classList.remove("close-mobile");
  }
}
