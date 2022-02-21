import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthorResponse } from '../book/model/paged-book-response';
import { ClientHttpService } from '../client-http.service';
import { AuthorSearchInput } from './model/author-search-input';
import { CreateAuthorRequest } from './model/create-author.request';
import { PagedAuthorResponse } from './model/paged-author-response';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  private authorUrl: string;
  constructor(private _httpService: ClientHttpService) {
    this.authorUrl = `${_httpService.baseUrl}/api/author`;
  }

  public get(input: AuthorSearchInput): Observable<PagedAuthorResponse> {
    let paramString = this._httpService.createQueryString(input)
    let queryUrl = this.authorUrl + "?" + paramString
    return this._httpService.get<PagedAuthorResponse>(queryUrl, this._httpService.httpOptions)
  }

  public create(name: string): Observable<AuthorResponse> {
    let author = <CreateAuthorRequest>{ name: name }
    return this._httpService.post<AuthorResponse>(this.authorUrl, author, this._httpService.httpOptions)
  }

  public delete(id: number): Observable<any> {

    return this._httpService.delete(`${this.authorUrl}/${id}`, this._httpService.httpOptions)
  }

}
