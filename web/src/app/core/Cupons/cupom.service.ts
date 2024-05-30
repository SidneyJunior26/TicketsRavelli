import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SecurityService } from '../Segurança/security.service';
import { Observable } from 'rxjs';
import { Desconto } from 'src/app/shared/models/desconto';

const url = 'http://localhost:11737/v1/descontos/';

@Injectable({
  providedIn: 'root',
})
export class CupomDescontoService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  consultatCuponsDescontoEvento(idEvento: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any>(url + 'evento/' + idEvento, { headers: headers });
  }

  validarCupom(cupom: string, idEvento: number): Observable<any> {
    return this.http.get<any>(url + `validar/${cupom}/${idEvento}`);
  }

  cadastrarCupom(desconto: Desconto): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.post(url, desconto, { headers: headers });
  }

  desativarCupom(cupomId: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.put<any>(url + 'desativar/' + cupomId, null, {
      headers: headers,
    });
  }

  ativarCupom(cupomId: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.put(url + 'ativar/' + cupomId, null, { headers: headers });
  }
}
