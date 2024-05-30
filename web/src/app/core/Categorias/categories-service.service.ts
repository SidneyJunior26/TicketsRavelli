import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Categoria } from 'src/app/shared/models/categoria';
import { SecurityService } from '../Segurança/security.service';

const url = 'http://localhost:11737/v1/categoria/';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  // GET
  consultarCategorias(): Observable<Categoria[]> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<Categoria[]>(url, {
      headers: headers,
    });
  }

  consultarCategoriasDoEvento(eventoId: number): Observable<Categoria[]> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<Categoria[]>(url + 'evento/' + eventoId, {
      headers: headers,
    });
  }

  consultarCategoriasFiltrado(
    idEvent: number,
    category: number,
    age: number,
    gender: number
  ): Observable<Categoria[]> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<Categoria[]>(
      url + idEvent + '/' + category + '/' + age + '/' + gender,
      {
        headers: headers,
      }
    );
  }

  consultarCategoriaId(idCategoria: number): Observable<Categoria> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<Categoria>(url + idCategoria, {
      headers: headers,
    });
  }

  // POST
  cadastrarCategoria(categoria: Categoria): Observable<Categoria> {
    const headers = this.securityService.getAuthentication();

    delete categoria.id;

    return this.http.post<Categoria>(url, categoria, { headers: headers });
  }

  // PUT
  editarCategoria(categoria: Categoria): Observable<Categoria> {
    const headers = this.securityService.getAuthentication();

    return this.http.put<Categoria>(
      url + 'atualizar/' + categoria.id,
      categoria,
      {
        headers: headers,
      }
    );
  }

  deletarCategoria(categoriaId: number) {
    const headers = this.securityService.getAuthentication();

    return this.http.delete(url + categoriaId, { headers: headers });
  }
}
