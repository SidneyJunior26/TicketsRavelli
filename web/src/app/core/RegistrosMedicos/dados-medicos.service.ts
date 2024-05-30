import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DadosMedicos } from 'src/app/shared/models/dadosMedicos';
import { SecurityService } from '../Segurança/security.service';

const url = 'http://localhost:11737/v1/RegistrosMedicos/';

@Injectable({
  providedIn: 'root',
})
export class DadosMedicosService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  consultarDadosMedicosPorAtletaId(
    athleteId: string
  ): Observable<DadosMedicos> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<DadosMedicos>(url + athleteId, {
      headers: headers,
    });
  }

  cadastrarDadosMedicos(dadosMedicos: DadosMedicos): Observable<DadosMedicos> {
    const headers = this.securityService.getAuthentication();

    return this.http.post<DadosMedicos>(url, dadosMedicos, {
      headers: headers,
    });
  }

  atualizarDadosMedicos(dadosMedicos: DadosMedicos): Observable<DadosMedicos> {
    const headers = this.securityService.getAuthentication();

    return this.http.put<DadosMedicos>(url, dadosMedicos, { headers: headers });
  }
}
