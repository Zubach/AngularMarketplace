import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
 

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
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
