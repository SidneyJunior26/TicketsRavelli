import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SecurityService } from '../Segurança/security.service';
import { PixInputModel } from 'src/app/shared/models/pixInputModel';

const url = 'http://localhost:11737/v1/pix/';

@Injectable({
  providedIn: 'root',
})
export class PixService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  gerarPix(pix: PixInputModel): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.post<any>(url, pix, {
      headers: headers,
    });
  }

  consultarPix(inscricaoId: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any>(url + inscricaoId, {
      headers: headers,
    });
  }

  validarPixNaoProcessados(idEvento: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get(url + 'confirmar-pagamentos/' + idEvento, {
      headers: headers,
    });
  }
}
