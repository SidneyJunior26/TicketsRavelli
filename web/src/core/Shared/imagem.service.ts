import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SecurityService } from '../Segurança/security.service';
import { Observable } from 'rxjs';

const url = 'http://localhost:11737/v1/events/';

@Injectable({
  providedIn: 'root',
})
export class ImagemService {
  headers: HttpHeaders;

  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {
    this.headers = this.securityService.getAuthentication();
  }

  salvarImagem(idEvento: number, file: File): Observable<any> {
    // Converte os dados da imagem para Base64
    const formData: FormData = new FormData();
    formData.append('file', file, idEvento.toString());

    return this.http.post<any>(url + `image/${idEvento}`, formData, {
      headers: this.headers,
    });
  }
}
