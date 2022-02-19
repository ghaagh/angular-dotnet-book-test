import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthorResponse } from '../book/model/paged-book-response';
import { AuthorSearchInput } from './model/author-search-input';
import { CreateAuthorRequest } from './model/create-author.request';
import { PagedAuthorResponse } from './model/paged-author-response';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
}

@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  private authorUrl: string;
  constructor(private _httpService: HttpClient) {
    this.authorUrl = "https://localhost:7294/api/author";
  }

  public get(input: AuthorSearchInput): Observable<PagedAuthorResponse> {
    let paramString = this._createQueryString(input)
    let queryUrl = this.authorUrl + "?" + paramString
    return this._httpService.get<PagedAuthorResponse>(queryUrl, httpOptions)
  }

  public create(name: string): Observable<AuthorResponse> {
    let author = <CreateAuthorRequest>{ name: name }
    return this._httpService.post<AuthorResponse>(this.authorUrl, author, httpOptions)
  }

  public delete(id: number): Observable<any> {

    return this._httpService.delete(`${this.authorUrl}/${id}`, httpOptions)
  }

  private _createQueryString(params): string {
    let esc = encodeURIComponent;
    let queryParams = Object.keys(params)
      .map(k => esc(k) + '=' + esc(params[k]))
      .join('&');
    return queryParams;
  }
}
