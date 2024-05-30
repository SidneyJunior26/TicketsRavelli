import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { SecurityService } from '../Segurança/security.service';
import { NovaSenha } from 'src/app/shared/models/Atleta/nova-senha';
import { AtualizaAtleta } from 'src/app/shared/models/Atleta/atualizar-atleta';

const url = 'http://localhost:11737/v1/Atletas/';
const securityUrl = 'http://localhost:11737/v1/Seguranca/';

@Injectable({
  providedIn: 'root',
})
export class AtletasService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  // GET
  verificarUsuarioExiste(cpf: string): Observable<any> {
    return this.http.get<any[]>(url + 'existe/' + cpf);
  }

  verificarUsuarioExisteEmail(email: string): Observable<any> {
    return this.http.get<any[]>(url + 'existe-email/' + email);
  }

  verificarUsuarioExisteRG(rg: string): Observable<any> {
    return this.http.get<any[]>(url + 'existe-rg/' + rg);
  }

  consultarTodosAtletas(): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any[]>(url, {
      headers: headers,
    });
  }

  filtrarAtletasPorCpf(cpf: string): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any[]>(url + 'cpf/' + cpf, {
      headers: headers,
    });
  }

  filtrarAtletasPorNome(name: string): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any[]>(url + 'nome/' + name, {
      headers: headers,
    });
  }

  consultarAtletaPorCPF(cpf: string): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any[]>(url + 'consultar/' + cpf, {
      headers: headers,
    });
  }

  consultarAtletaPorId(id: string): Observable<any> {
    return this.http.get<any[]>(url + 'id/' + id);
  }

  // verificaPrimeiroAcesso(cpfAtleta: string): Observable<any> {
  //   return this.http.get<any>(securityUrl + 'primeiro-acesso/' + cpfAtleta);
  // }

  // POST
  login(cpf: string, password: string): Observable<any> {
    return this.http
      .post<any>(securityUrl + 'login', {
        cpf: cpf,
        password: password,
      })
      .pipe(
        map((token) => {
          return JSON.stringify(token);
        })
      );
  }

  // POST
  cadastrarAtleta(athlete: Atleta): Observable<any> {
    delete athlete.id;
    delete athlete.nivel;

    return this.http.post<any>(url, athlete);
  }

  cadastrarAtletaAdm(athlete: Atleta): Observable<any> {
    const headers = this.securityService.getAuthentication();

    delete athlete.id;
    delete athlete.nivel;

    return this.http.post<any>(url, athlete, {
      headers: headers,
    });
  }

  // PUT
  atualizarAtleta(
    atleta: AtualizaAtleta,
    idAtleta: string
  ): Observable<AtualizaAtleta> {
    const headers = this.securityService.getAuthentication();

    return this.http.put<AtualizaAtleta>(url + idAtleta, atleta, {
      headers: headers,
    });
  }

  atualizarCamisasAtleta(
    cpf: string,
    camisa: string,
    camisaCiclismo: string
  ): Observable<any> {
    const headers = this.securityService.getAuthentication();

    return this.http.put<Atleta>(
      url + 'atualizar-camisas/' + cpf,
      { camisa: camisa, camisaCiclismo: camisaCiclismo },
      {
        headers: headers,
      }
    );
  }

  atualizarSenha(novaSenha: NovaSenha) {
    const headers = this.securityService.getAuthentication();

    return this.http.put<NovaSenha>(securityUrl + 'alterar-senha', novaSenha, {
      headers: headers,
    });
  }

  deletarAtleta(atletaId: string) {
    const headers = this.securityService.getAuthentication();

    return this.http.delete(url + atletaId, { headers: headers });
  }
}
