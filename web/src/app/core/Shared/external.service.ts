import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

const url = 'https://viacep.com.br/ws/CEP/json/';

@Injectable({
  providedIn: 'root',
})
export class ExternalService {
  constructor(private http: HttpClient) {}

  consultarEndereçoPorCEP(cep: string): Observable<any> {
    return this.http.get<any>(url.replace('CEP', cep));
  }
}
