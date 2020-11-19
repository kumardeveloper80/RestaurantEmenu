import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MenuComponent } from './components/menu/menu.component';
import { MenuItemDetailsComponent  } from './components/menu-item-details/menu-item-details.component';
import { SurveyFormComponent  } from './components/survey-form/survey-form.component';


const routes: Routes = [
  { path: 'Emenu/:code', component: MenuComponent },
  { path: 'Item/:code/:id', component: MenuItemDetailsComponent },
  { path: 'Survey/:id', component: SurveyFormComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
