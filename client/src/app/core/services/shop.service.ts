import { HttpClient, HttpParams } from '@angular/common/http';
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

  getProducts(brands?: string[], types?: string[], sort?: string) {
    let params = new HttpParams();

    if (brands && brands.length > 0) {
      params = params.append('brands', brands.join(','));
    }
    if (types && types.length > 0) {
      params = params.append('types', types.join(','));
    }

    if(sort) {
      params = params.append('sort', sort);
    }
    params = params.append('pageSize', 20);
    return this.http.get<Pagination<Product>>(
      this.baseUrl + 'products', {params}
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
