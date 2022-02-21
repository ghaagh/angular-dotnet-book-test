import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ClientHttpService extends HttpClient {
  public baseUrl = 'https://localhost:7294';
  public httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }
  public createQueryString(params): string {
    let esc = encodeURIComponent;
    let queryParams = Object.keys(params)
      .map(k => esc(k) + '=' + esc(params[k]))
      .join('&');
    return queryParams;
  }
}
