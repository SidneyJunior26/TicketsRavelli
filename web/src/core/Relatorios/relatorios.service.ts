import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SecurityService } from '../Segurança/security.service';

const url = 'http://localhost:11737/v1/relatorios/';

@Injectable({
  providedIn: 'root',
})
export class RelatoriosService {
  headers: HttpHeaders;

  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {
    this.headers = this.securityService.getAuthentication();
  }

  consultarRelatorioInscricoesEvento(idEvento: number): Observable<any> {
    return this.http.get<any>(url + idEvento, { headers: this.headers });
  }

  consultarRelatorioInscricoesEventoPorCategoria(
    idEvento: number
  ): Observable<any> {
    return this.http.get<any>(url + `inscritos-categoria/${idEvento}`, {
      headers: this.headers,
    });
  }

  consultarRelatorioTotalInscritosCategoria(idEvento: number): Observable<any> {
    return this.http.get<any>(url + `total-inscritos-categoria/${idEvento}`, {
      headers: this.headers,
    });
  }

  consultarRelatorioTotalInscritosEfetivados(
    idEvento: number
  ): Observable<any> {
    return this.http.get<any>(url + `total-efetivados/${idEvento}`, {
      headers: this.headers,
    });
  }

  consultarRelatorioTotalCamisasCategoria(idEvento: number): Observable<any> {
    return this.http.get<any>(url + `total-camisetas/${idEvento}`, {
      headers: this.headers,
    });
  }
}
