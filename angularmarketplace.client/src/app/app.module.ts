import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {  registerLocaleData } from '@angular/common';
 

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ProductListComponent } from './product-list/product-list.component';
import { ProductItemComponent } from './product-list/product-item/product-item.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HeaderComponent } from './header/header.component';
import { MobileUserMenuComponent } from './header/mobile-user-menu/mobile-user-menu.component';
import { SearchbarComponent } from './header/searchbar/searchbar.component';
import { ProductDetailComponent } from './product-list/product-item/product-detail/product-detail.component';
import { SearchresultComponent } from './search/searchresult/searchresult.component';
import { CategoriesListComponent } from './categories-list/categories-list.component';
import { MainpageComponent } from './mainpage/mainpage.component';
import { CategorypageComponent } from './categorypage/categorypage.component';

import {ImageLoaderDirective} from './Directives/image-loader.directive';
import { WishlistComponent } from './user/cabinet/wishlist/wishlist.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { SellerMenuComponent } from './user/seller/seller-menu/seller-menu.component';
import { SellerProductsListComponent } from './user/seller/seller-menu/seller-products-list/seller-products-list.component';
import { FormsModule } from '@angular/forms';
import { CreateProductComponent } from "./user/seller/seller-menu/create-product/create-product.component";
import { CategoriesMenuComponent } from './Areas/admin/cabinet/categories-menu/categories-menu.component';
import { CategoriesTableComponent } from './Areas/admin/cabinet/categories-menu/categories-table/categories-table.component';
import { AdminCabinetComponent } from './Areas/admin/cabinet/cabinet.component';
import { HomeComponent } from './home/home.component';
import { AddCategoryComponent } from "./Areas/admin/cabinet/categories-menu/add-category/add-category.component";
import { ToastContainer } from "./toast-container/toast-container.component";
import { ModeratorCabinetComponent } from './Areas/moderator/cabinet/cabinet.component';




@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent,
    ProductItemComponent,
    HeaderComponent,
    MobileUserMenuComponent,
    SearchbarComponent,
    ProductDetailComponent,
    SearchresultComponent,
    CategoriesListComponent,
    MainpageComponent,
    CategorypageComponent,
    ImageLoaderDirective,
    WishlistComponent,
    SellerMenuComponent,
    SellerProductsListComponent,
    CategoriesMenuComponent,
    CategoriesTableComponent,
    AdminCabinetComponent,
    HomeComponent,
    ModeratorCabinetComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    NgbModule,
    FormsModule,
    CreateProductComponent,
    AddCategoryComponent,
    ToastContainer
],
  providers: [{
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi:true
  }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
