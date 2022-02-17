import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { BookSearchInput } from './model/book-search-input';
import { PagedBookResponse } from './model/paged-book-response';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}
@Injectable({
  providedIn: 'root'
})
export class BookService {
  private bookUrl: string;
  constructor(private _httpService: HttpClient) {
    this.bookUrl = "https://localhost:7294/api/book";
  }

  public get(input: BookSearchInput): Observable<PagedBookResponse> {
    let paramString = this.createQueryString(input);
    let queryUrl = this.bookUrl + "?" + paramString;
    return this._httpService.get<PagedBookResponse>(queryUrl, httpOptions);
  }

  createQueryString(params): string {
    let esc = encodeURIComponent;
    let queryParams = Object.keys(params)
      .map(k => esc(k) + '=' + esc(params[k]))
      .join('&');
    return queryParams;
  }
}
