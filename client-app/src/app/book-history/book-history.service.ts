import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedBookHistoryResponse } from './model/paged-book-response';
import { BookSearchInput } from '../book/model/book-search-input';
import { ClientHttpService } from '../client-http.service';


@Injectable({
  providedIn: 'root'
})
export class BookHistoryService {
  private bookHistoryUrl: string;
  constructor(private _httpService: ClientHttpService) {
    this.bookHistoryUrl =`${_httpService.baseUrl}/api/bookhistory`;
  }

  public get(bookId: number, input: BookSearchInput): Observable<PagedBookHistoryResponse> {
    let paramString = this._httpService.createQueryString(input);
    let queryUrl = `${this.bookHistoryUrl}/${bookId}` + "?" + paramString;
    return this._httpService.get<PagedBookHistoryResponse>(queryUrl, this._httpService.httpOptions);
  }


}
