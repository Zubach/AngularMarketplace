import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductDetailComponent } from './product-list/product-item/product-detail/product-detail.component';
import { SearchresultComponent } from './search/searchresult/searchresult.component';
import { MainpageComponent } from './mainpage/mainpage.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { WishlistComponent } from './user/cabinet/wishlist/wishlist.component';
import { SellerMenuComponent } from './user/seller/seller-menu/seller-menu.component';
import { sellerGuard } from './guards/seller.guard';
import { adminGuard } from './guards/admin.guard';
import { AdminCabinetComponent } from './Areas/admin/cabinet/cabinet.component';
import { CategoriesMenuComponent } from './Areas/admin/cabinet/categories-menu/categories-menu.component';
import { HomeComponent } from './home/home.component';
import { nonAuthGuard } from './guards/non-auth.guard';
import { ModeratorCabinetComponent } from './Areas/moderator/cabinet/cabinet.component';
import { ModeratorProductsComponent } from './Areas/moderator/cabinet/products/products.component';
import { moderatorOrAdminGuard } from './guards/moderator-or-admin.guard';

const routes: Routes = [

  {
    path: 'admin-cabinet',
    canActivate: [adminGuard],
    component: AdminCabinetComponent,
    children:[
      {path: 'categories-menu',component:CategoriesMenuComponent}
    ]
  },
  {
    path: 'moderator',
    canActivate: [moderatorOrAdminGuard],
    component: ModeratorCabinetComponent,
    children:[
      {path: 'products', component:ModeratorProductsComponent}
    ]
  },
  {
    path: '', 
    component: HomeComponent, 
    children:[
      {path: '',component:MainpageComponent,pathMatch:'full'},
      {path: 'login',component:LoginComponent,pathMatch:'full',canActivate:[nonAuthGuard]},
      {path: 'registration',component:RegistrationComponent, pathMatch:'full',canActivate:[nonAuthGuard]},
      {path: ':url_title/:mask', component: SearchresultComponent }
    ]

  },
  {path: 'sellermenu', component:SellerMenuComponent, pathMatch:'full',canActivate:[sellerGuard]}, 
  {path: 'wishlist', component:WishlistComponent, pathMatch:'full'},
  {path: 'product/:id',component: ProductDetailComponent},
  
  {path: '**',redirectTo: '/'}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
