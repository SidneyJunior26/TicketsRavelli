import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from 'src/app/shared/models/evento';
import { SecurityService } from '../Segurança/security.service';
import { Pacotes } from 'src/app/shared/models/pacotes';

const url = 'http://localhost:11737/v1/events/';

@Injectable({
  providedIn: 'root',
})
export class EventosService {
  constructor(
    private http: HttpClient,
    private securityService: SecurityService
  ) {}

  /* GET */
  getAllEvents(): Observable<any[]> {
    const headers = this.securityService.getAuthentication();

    return this.http.get<any[]>(url, {
      headers: headers,
    });
  }

  consultarProximosEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(url + 'upcoming');
  }

  getAllEventsActives(): Observable<Evento[]> {
    return this.http.get<Evento[]>(url + 'actives');
  }

  consultarEventoPeloId(id: number): Observable<Evento> {
    return this.http.get<Evento>(url + id);
  }

  consultarPacotesPorEvento(eventoId: number): Observable<Pacotes> {
    return this.http.get<Pacotes>(url + 'packages/' + eventoId);
  }

  consultarValorPacote(inscricaoId: number): Observable<any> {
    return this.http.get<any>(url + 'package-value/' + inscricaoId);
  }

  /* POST */
  cadastrarEvento(evento: Evento): Observable<number> {
    delete evento.id;
    delete evento.notifications;
    delete evento.isValid;

    const headers = this.securityService.getAuthentication();

    return this.http.post<number>(url, evento, { headers: headers });
  }

  /* PUT */
  atualizarEvento(evento: Evento): Observable<Evento> {
    var id = evento.id!;

    delete evento.id;
    delete evento.notifications;
    delete evento.isValid;

    const headers = this.securityService.getAuthentication();

    return this.http.put<Evento>(url + id, evento, { headers: headers });
  }

  cadastrarImagem(eventoId: number, imagem: File): Observable<any> {
    return this.http.post<any>(url + 'image/' + eventoId, imagem);
  }

  /* DELETE */
  deletarEventoPeloId(id: number) {
    const headers = this.securityService.getAuthentication();

    return this.http.delete(url + id, { headers: headers });
  }
}
