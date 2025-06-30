import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductDetailComponent } from './product-list/product-item/product-detail/product-detail.component';
import { SearchresultComponent } from './search/searchresult/searchresult.component';
import { MainpageComponent } from './mainpage/mainpage.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';

const routes: Routes = [
  {path: '', component: MainpageComponent},
  {path: 'registration',component:RegistrationComponent, pathMatch:'full'},
  {path: 'login',component:LoginComponent,pathMatch:'full'},
  {path: 'product/:id',component: ProductDetailComponent},
  {path: ':url_title/:mask', component: SearchresultComponent},
  {path: '**',redirectTo: '/'}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
