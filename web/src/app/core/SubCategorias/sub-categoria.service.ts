import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SecurityService } from '../Segurança/security.service';
import { SubCategoria } from 'src/app/shared/models/subCategoria';
import { Observable } from 'rxjs';

const url = 'http://localhost:11737/v1/categoria/';

@Injectable({
  providedIn: 'root',
})
export class SubCategoriaService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  consultarSubCategoria(id: number): Observable<SubCategoria> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<SubCategoria>(url + id, {
      headers: headers,
    });
  }

  atualizarSubcategoria(subCategoria: SubCategoria): Observable<SubCategoria> {
    const id = subCategoria.id;

    delete subCategoria.id;

    const headers = this.securityService.getAuthentication();

    return this.http.put<SubCategoria>(url + id, subCategoria, {
      headers: headers,
    });
  }
}
