import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  private readonly baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  types: string[] = [];
  brands: string[] = [];

  getProducts() {
    return this.http.get<Pagination<Product>>(
      this.baseUrl + 'products?pageSize=20'
    );
  }

  getBrands() {
    if (this.brands.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: (r) => (this.brands = r),
    });
  }

  getTypes() {
    if (this.types.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
      next: (r) => (this.types = r),
    });
  }
}
