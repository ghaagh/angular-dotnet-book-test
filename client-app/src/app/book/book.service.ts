import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { BookSearchInput } from './model/book-search-input';
import { BookResponse, PagedBookResponse } from './model/paged-book-response';
import { CreateBookRequest } from './model/create-book-request';
import { ClientHttpService } from '../client-http.service';


@Injectable({
  providedIn: 'root'
})
export class BookService {
  private bookUrl: string;
  constructor(private _httpService: ClientHttpService) {
    this.bookUrl = `${_httpService.baseUrl}/api/book`;
  }

  public get(input: BookSearchInput): Observable<PagedBookResponse> {
    let paramString = this._createQueryString(input);
    let queryUrl = this.bookUrl + "?" + paramString;
    return this._httpService.get<PagedBookResponse>(queryUrl, this._httpService.httpOptions);
  }

  public getById(input: number): Observable<BookResponse> {
    let queryUrl = this.bookUrl + "/" + input;
    return this._httpService.get<BookResponse>(queryUrl, this._httpService.httpOptions);
  }

  public delete(id: number): Observable<any> {

    return this._httpService.delete(`${this.bookUrl}/${id}`, this._httpService.httpOptions)
  }

  public create(request: CreateBookRequest): Observable<PagedBookResponse> {
    return this._httpService.post<PagedBookResponse>(this.bookUrl, request, this._httpService.httpOptions)
  }

  public edit(id: number, request: CreateBookRequest): Observable<PagedBookResponse> {
    return this._httpService.put<PagedBookResponse>(`${this.bookUrl}/${id}`, request, this._httpService.httpOptions)
  }

  private _createQueryString(params): string {
    let esc = encodeURIComponent;
    let queryParams = Object.keys(params)
      .map(k => esc(k) + '=' + esc(params[k]))
      .join('&');
    return queryParams;
  }
}
