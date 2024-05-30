import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Regulamento } from 'src/app/shared/models/regulamento';
import { SecurityService } from '../Segurança/security.service';

const url = 'http://localhost:11737/v1/regulamentos/';

@Injectable({
  providedIn: 'root',
})
export class RegulamentoService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  consultarRegulamentoPorEvento(eventId: number): Observable<Regulamento> {
    return this.http.get<Regulamento>(url + eventId);
  }

  cadastrarRegulamento(regulamento: Regulamento): Observable<Regulamento> {
    const headers = this.securityService.getAuthentication();

    return this.http.post<Regulamento>(url, regulamento, { headers: headers });
  }

  atualizarRegulamento(regulamento: Regulamento): Observable<Regulamento> {
    const idEvento = regulamento.idEvento;
    const headers = this.securityService.getAuthentication();

    delete regulamento.idEvento;

    return this.http.put<Regulamento>(url + idEvento, regulamento, {
      headers: headers,
    });
  }
}
