import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SecurityService } from '../Segurança/security.service';
import { BoletoInputModel } from 'src/app/shared/models/boletoInputModel';

const url = 'http://localhost:11737/v1/boleto/';

@Injectable({
  providedIn: 'root',
})
export class BoletoService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  gerarBoleto(idInscricao: number, boleto: BoletoInputModel): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.post<any>(url + idInscricao, boleto, {
      headers: headers,
    });
  }
}
