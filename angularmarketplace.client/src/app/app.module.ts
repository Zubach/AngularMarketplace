import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { IMAGE_CONFIG, IMAGE_LOADER, ImageLoaderConfig, NgOptimizedImage } from '@angular/common';
 

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
import { environment } from '../environments/environment.development';
import {ImageLoaderDirective} from './Directives/image-loader.directive';
import { WishlistComponent } from './user/cabinet/wishlist/wishlist.component';
import { TokenInterceptor } from './interceptors/token.interceptor';


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
    WishlistComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    NgbModule,
    NgOptimizedImage,
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
