import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { AtletasService } from 'src/app/core/Atletas/atletas.service';
import { EventosService } from 'src/app/core/Eventos/events.service';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { LoginComponent } from 'src/app/shared/login/login.component';
import { ValoresEventoComponent } from '../valores-evento/valores-evento.component';
import { SubscriptionsService } from 'src/app/core/Inscricoes/inscricoes.service';
import { MensagemModalComponent } from 'src/app/shared/mensagem-modal/mensagem-modal.component';

@Component({
  selector: 'app-events-all-active-list',
  templateUrl: './events-all-active-list.component.html',
  styleUrls: ['./events-all-active-list.component.css'],
})
export class EventsAllActiveListComponent implements OnInit {
  loadingEventosAbertos: boolean = true;
  loadingProximosEventos: boolean = true;
  loadingInscricao: boolean = false;

  events: any[] = [];
  eventsOpen: any[] = [];
  constructor(
    private service: EventosService,
    private snackBar: MatSnackBar,
    private athleteService: AtletasService,
    private inscricoesService: SubscriptionsService,
    private router: Router,
    private dialog: MatDialog,
    private securityService: SecurityService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const idAfiliado = this.activatedRoute.snapshot.params['idAfiliado'];

    if (idAfiliado != null) localStorage.setItem('affiliateId', idAfiliado);

    this.consultarEventosAtivos();
    this.consultarProximosEventos();
  }

  private consultarProximosEventos() {
    this.service.consultarProximosEventos().subscribe(
      (events) => {
        this.events.push(...events);
        this.loadingProximosEventos = false;
      },
      (error) => {
        this.loadingProximosEventos = false;
      }
    );
  }

  private consultarEventosAtivos() {
    this.service.getAllEventsActives().subscribe(
      (events) => {
        this.eventsOpen.push(...events);
        this.loadingEventosAbertos = false;
      },
      () => {
        this.loadingEventosAbertos = false;
      }
    );
  }

  changeView(eventId: number): void {
    const token = this.securityService.getToken();

    if (token != null)
      var userInfo = this.securityService.getDecodedAccessToken(token);

    if (userInfo != null) {
      this.verificaAtletaInscrito(userInfo.cpf, eventId);
    } else {
      localStorage.setItem('eventId', eventId.toString());
      this.dialog.open(LoginComponent);
    }
  }

  private verificaAtletaInscrito(cpfAtleta: string, eventId: number) {
    this.loadingInscricao = true;

    this.inscricoesService
      .checkIfAthleteSubscribedByEvent(cpfAtleta, eventId)
      .subscribe(
        () => {
          this.dialog.open(MensagemModalComponent, {
            width: '600px',
            data: {
              mensagem: 'Você já esta inscrito neste evento',
              icone: 'info',
            },
          });
        },
        (error) => {
          if (error.status == 404) {
            this.router.navigateByUrl('eventos/' + eventId);
          }
        },
        () => {
          this.loadingInscricao = false;
        }
      );
  }

  abrirValoresPacotesEventos(eventoId: number, nomeEvento: string) {
    this.dialog.open(ValoresEventoComponent, {
      data: {
        eventoId: eventoId,
        nomeEvento: nomeEvento,
      },
    });
  }

  abrirInscricoes(eventoId: number) {
    this.router.navigateByUrl('eventos/inscritos/' + eventoId);
  }

  verifyIfUserExists(cpf: string, eventId: string) {
    if (cpf == null || cpf == '') {
      this.openMessage('Informe seu CPF/CNPJ');
      return;
    }

    cpf = cpf.replace('.', '').replace('.', '').replace('-', '');

    this.athleteService.verificarUsuarioExiste(cpf).subscribe(
      () => {
        this.logIn(cpf, eventId);
      },
      (error) => {
        if (error.status == 404)
          this.router.navigateByUrl('eventos/' + eventId);
        if (error.status == 401) {
          this.securityService.logOutToken();
          this.logIn(cpf, eventId);
        }
      }
    );
  }

  private logIn(cpf: string, eventId: string) {
    localStorage.setItem('eventId', eventId);
    localStorage.setItem('cpf', cpf);
    this.dialog.open(LoginComponent);
  }

  private openMessage(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
