import { Injectable } from '@angular/core';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';

@Injectable({
  providedIn: 'root',
})
export class WebsocketsService {
  private socket$: WebSocketSubject<string>;
  private readonly SERVER_URL =
    'ws:// sidneyjunior.com.br/api/v1/api/pix-recebido';

  constructor() {
    this.socket$ = webSocket(this.SERVER_URL);
  }

  getMessages() {
    return this.socket$.asObservable();
  }
}
