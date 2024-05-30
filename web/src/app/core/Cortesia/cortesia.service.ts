import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Evento } from 'src/app/shared/models/evento';
import { SecurityService } from '../Segurança/security.service';
import { Observable } from 'rxjs';

const url = 'http://localhost:11737/v1/cortesias/';

@Injectable({
  providedIn: 'root',
})
export class CortesiaService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  consultarCupomCortesia(idEvento: number, cupom: string): Observable<any> {
    return this.http.get<any>(url + 'validar/' + idEvento + '/' + cupom);
  }

  consultarCupomCortesiaEvento(idEvento: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any>(url + 'evento/' + idEvento, { headers: headers });
  }

  alterarStatus(cupom: string): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.put(url + 'status/' + cupom, null, { headers: headers });
  }

  desativarCupomCortesia(idEvento: number, cupom: string): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.put(url + 'desativar/' + idEvento + '/' + cupom, null, {
      headers: headers,
    });
  }

  cadastrarCupomCortesia(evento: Evento): Observable<any> {
    const headers = this.securityService.getAuthentication();

    const body = {
      idEvento: evento.id!,
    };

    return this.http.post<Evento[]>(url, body, {
      headers: headers,
    });
  }

  deletarCupom(cupom: string): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.delete(url + cupom, { headers: headers });
  }
}
