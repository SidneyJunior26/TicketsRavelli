import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SecurityService } from '../Segurança/security.service';
import { Observable } from 'rxjs';
import { Afiliado } from 'src/app/shared/models/afiliado';

const url = 'http://localhost:11737/v1/Afiliado/';

@Injectable({
  providedIn: 'root',
})
export class AfiliadoService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  consultarTodosAfiliados(): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any[]>(url, {
      headers: headers,
    });
  }

  consultarAfiliadosEvento(idEvento: number): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any[]>(url + 'evento/' + idEvento, {
      headers: headers,
    });
  }

  cadastrarAfiliado(afiliado: Afiliado) {
    const headers = this.securityService.getAuthentication();

    return this.http.post<any>(url, afiliado, { headers: headers });
  }

  atualizarAfiliado(afiliadoId: string, afiliado: Afiliado) {
    const headers = this.securityService.getAuthentication();

    return this.http.put<any>(url + afiliadoId, afiliado, { headers: headers });
  }

  deletarAfiliado(afiliadoId: string) {
    const headers = this.securityService.getAuthentication();

    return this.http.delete(url + afiliadoId, { headers: headers });
  }
}
