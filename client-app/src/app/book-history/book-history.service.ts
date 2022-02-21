import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedBookHistoryResponse } from './model/paged-book-response';
import { BookSearchInput } from '../book/model/book-search-input';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}
@Injectable({
  providedIn: 'root'
})
export class BookHistoryService {
  private bookHistoryUrl: string;
  constructor(private _httpService: HttpClient) {
    this.bookHistoryUrl = "https://localhost:7294/api/bookhistory";
  }

  public get(bookId: number, input: BookSearchInput): Observable<PagedBookHistoryResponse> {
    let paramString = this._createQueryString(input);
    let queryUrl = `${this.bookHistoryUrl}/${bookId}` + "?" + paramString;
    return this._httpService.get<PagedBookHistoryResponse>(queryUrl, httpOptions);
  }

  private _createQueryString(params): string {
    let esc = encodeURIComponent;
    let queryParams = Object.keys(params)
      .map(k => esc(k) + '=' + esc(params[k]))
      .join('&');
    return queryParams;
  }
}
