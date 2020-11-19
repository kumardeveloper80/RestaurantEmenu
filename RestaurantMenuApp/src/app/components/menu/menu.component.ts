import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuService } from '../menu/menu.service';
import { HeaderService } from "../header/header.service";

import { OwlOptions } from 'ngx-owl-carousel-o';


@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  styles: ['.test { color: colorCode; }'],
})
export class MenuComponent implements OnInit {
  lang = localStorage.getItem('lang');
  menuSchedule: any;
  selectedCategory: any;
  selectedMenuItems: [];
  code: any;
  customOptions: OwlOptions = {
    margin: 10,
    nav: false,
    dots: false,
    items: 3,
    rtl: localStorage.getItem('lang') === 'AR' ? true : false,
    responsive: {
      0: {
        items: 2,
      },
      381: {
        items: 3,
      }
    }
  }

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _menuService: MenuService,
    private _headerService: HeaderService) { }

  ngOnInit(): void {
    (<HTMLInputElement>document.getElementById('backBtnLi')).classList.add("d-none");
    (<HTMLInputElement>document.getElementById('feedbackBtn')).classList.remove("d-none");
    this.getMenu();

  }

  async getMenu() {
    this.code = this._route.snapshot.paramMap.get('code');
    this._menuService.getMenu(this.code).subscribe((res) => {
      if (res["status"] === 1) {
        this.menuSchedule = res["dataenum"];
        if (this.menuSchedule.categories.length > 0) {
          this._headerService.updateStoreId(this.menuSchedule.storeGuid)
          this._headerService.updateConceptTheme(this.menuSchedule.logo, this.menuSchedule.colorCode, this.menuSchedule.feedBackIcon);

          sessionStorage.setItem("storeGuid", this.menuSchedule.storeGuid);
          sessionStorage.setItem("logo", this.menuSchedule.logo);
          sessionStorage.setItem("colorCode", this.menuSchedule.colorCode);
          sessionStorage.setItem("feedBackIcon", this.menuSchedule.feedBackIcon);

          let tempCategoryId = sessionStorage.getItem("selectedCategoryId");
          if (tempCategoryId !== null && parseInt(tempCategoryId) > 0) {
            this.selectedCategory = this.menuSchedule.categories.find(x => x.id === parseInt(tempCategoryId));
            if (this.selectedCategory === undefined) {
              this.selectedCategory = this.menuSchedule.categories[0];
            }
          } else {
            this.selectedCategory = this.menuSchedule.categories[0];
          }

          this.getMenuItem();
         //setTimeout(this.setUI,2000);
        }
      } else {
        alert(res["message"])
      }
    });
  }

  onSlideClick(menuItem) {
    this.selectedCategory = menuItem;
    sessionStorage.setItem("selectedCategoryId", menuItem.id)
    this.getMenuItem();
    //setTimeout(this.setUI,1000);

    // window.scrollTo(0,document.querySelector(".section").scrollHeight);
    // console.log(document.querySelector(".section").scrollHeight);
  }

  getMenuItem() {
    this.selectedMenuItems = this.menuSchedule.menu.menuItems.filter(obj => {

      return obj.categoryId === this.selectedCategory.id;

    })
  }

  setUI() {

    var elem = (document.compatMode === "CSS1Compat") ?
      document.documentElement :
      document.body;

    var totalHeight = elem.clientHeight;
   
    var navbarHeight = document.getElementById("navbar").clientHeight;

    console.log(navbarHeight);

    var catbarHeight = document.getElementById("catsection").clientHeight; 

    console.log(catbarHeight);
    var subHeight = (58 + navbarHeight + catbarHeight);
    var listHeight = totalHeight - subHeight;

    //document.getElementById("menuitems").style.height =  listHeight.toString() + "px";
   
  }

  calculateClasses() {
    return {
      test: true,
    };
  }

  onMenuItem(menuItem) {
    sessionStorage.setItem("selectedCategoryId", menuItem.categoryId)
    this._router.navigateByUrl('/Item/' + this.code + '/' + menuItem.id);
  }
}
