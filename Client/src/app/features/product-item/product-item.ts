import { Component, Input } from '@angular/core';
import { Product } from '../../shared/models/product';
import { MatCardContent, MatCardModule } from '@angular/material/card';
import { CurrencyPipe } from '@angular/common';
import { MatAnchor } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: 'app-product-item',
  imports: [
    MatCardModule,
    MatCardContent,
    CurrencyPipe,
    MatAnchor,
    MatIconModule
],
  templateUrl: './product-item.html',
  styleUrl: './product-item.scss'
})
export class ProductItem {
  @Input() product?: Product;
}
