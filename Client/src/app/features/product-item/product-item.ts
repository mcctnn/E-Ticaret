import { Component, inject, Input } from '@angular/core';
import { Product } from '../../shared/models/product';
import { MatCardContent, MatCardModule } from '@angular/material/card';
import { CurrencyPipe } from '@angular/common';
import { MatAnchor } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { RouterLink } from '@angular/router';
import { CartService } from '../../core/services/cart.service';

@Component({
  selector: 'app-product-item',
  imports: [
    MatCardModule,
    MatCardContent,
    CurrencyPipe,
    MatAnchor,
    MatIconModule,
    RouterLink
  ],
  templateUrl: './product-item.html',
  styleUrl: './product-item.scss'
})
export class ProductItem {
  @Input() product?: Product;
  cartService = inject(CartService);
}
