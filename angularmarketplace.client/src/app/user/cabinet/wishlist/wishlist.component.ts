import { Component, OnInit } from '@angular/core';
import { WishlistService } from '../../../services/wishlist.service';
import { Wishlist } from '../../../Models/User/wishlist.model';

@Component({
  selector: 'app-wishlist-list',
  templateUrl: './wishlist.component.html',
  styleUrl: './wishlist.component.css'
})
export class WishlistComponent implements OnInit {
  wishlistList!:Wishlist[];
  constructor(private wishlistService: WishlistService){}

  ngOnInit(): void {
    this.wishlistService.getUserWishlists().subscribe((response)=>{
      console.log(response);
    });
  }

}
