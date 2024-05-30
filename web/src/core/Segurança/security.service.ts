import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import jwtDecode from 'jwt-decode';
import { Observable } from 'rxjs';
import {
  EsqueciSenha,
  ManagerNovaSenha,
  NovaSenhaCodigo,
} from 'src/app/shared/models/Atleta/nova-senha';

const url = 'http://localhost:11737/v1/seguranca/';

@Injectable({
  providedIn: 'root',
})
export class SecurityService {
  tokenKey = 'currentUser';

  constructor(private router: Router, private http: HttpClient) {}

  getDecodedAccessToken(token: string): any {
    try {
      return jwtDecode(token);
    } catch (Error) {
      return null;
    }
  }

  getToken(): string {
    let tokenValue = '';

    if (localStorage.getItem(this.tokenKey) != null)
      tokenValue = localStorage.getItem(this.tokenKey)!;

    return tokenValue;
  }

  logOutToken() {
    localStorage.removeItem(this.tokenKey);
    this.router.navigateByUrl('/');
  }

  getAuthentication(): HttpHeaders {
    if (this.getToken() != '')
      return new HttpHeaders().set(
        'Authorization',
        'Bearer ' + JSON.parse(this.getToken()).token
      );
    else return new HttpHeaders();
  }

  enviarCodigoSegurancaPorId(id: string): Observable<any> {
    return this.http.post<any>(url + 'enviar-codigo/id' + id, null);
  }

  enviarCodigoSegurancaPorCpfEmail(cpfEmail: string): Observable<any> {
    return this.http.post<any>(url + 'enviar-codigo/' + cpfEmail, null);
  }

  alterarPrimeiraSenha(novaSenhaCodigo: NovaSenhaCodigo): Observable<any> {
    return this.http.put<NovaSenhaCodigo>(url + 'nova-senha', novaSenhaCodigo);
  }

  managerAlterarSenha(managerNovaSenha: ManagerNovaSenha): Observable<any> {
    const headers = this.getAuthentication();

    return this.http.put<NovaSenhaCodigo>(
      url + 'manager/nova-senha',
      managerNovaSenha,
      {
        headers: headers,
      }
    );
  }

  esqueciSenha(esqueciSenha: EsqueciSenha): Observable<any> {
    return this.http.put<EsqueciSenha>(url + 'esqueci-senha', esqueciSenha);
  }
}
