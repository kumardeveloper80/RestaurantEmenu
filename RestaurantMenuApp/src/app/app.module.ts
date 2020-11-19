import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule,ReactiveFormsModule }   from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CarouselModule } from 'ngx-owl-carousel-o';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './components/menu/menu.component';
import { MenuItemDetailsComponent } from './components/menu-item-details/menu-item-details.component';
import { HeaderComponent } from './components/header/header.component';
import { SurveyFormComponent } from './components/survey-form/survey-form.component';
import { MatSliderModule } from '@angular/material/slider';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    MenuItemDetailsComponent,
    HeaderComponent,
    SurveyFormComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    CarouselModule,
    MatSliderModule,
    NgbModule,
   
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
