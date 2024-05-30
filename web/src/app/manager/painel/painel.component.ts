import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatTabGroup } from '@angular/material/tabs';
import { AtletasService } from 'src/app/core/Atletas/atletas.service';
import { EventosService } from 'src/app/core/Eventos/events.service';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { ImageUploadComponent } from 'src/app/shared/image-upload/image-upload.component';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { Evento } from 'src/app/shared/models/evento';
import { CategoriesComponent } from '../categories/categories.component';
import { RegulamentosComponent } from '../regulamentos/regulamentos.component';
import { MensagemConfirmacaoComponent } from 'src/app/shared/mensagem-confirmacao/mensagem-confirmacao.component';
import { DadosMedicosComponent } from '../dados-medicos/dados-medicos.component';
import { LoginComponent } from 'src/app/shared/login/login.component';
import { CadastroInscricaoComponent } from '../cadastro-inscricao/cadastro-inscricao/cadastro-inscricao.component';
import { CortesiaComponent } from '../cortesia/cortesia.component';
import { RelatoriosComponent } from '../relatorios/relatorios.component';
import { CuponsComponent } from '../cupons/cupons.component';
import { AtletaComponent } from '../atleta/atleta.component';
import { EventoComponent } from '../evento/evento.component';
import { EfetivarInscricaoComponent } from '../efetivar-inscricao/efetivar-inscricao.component';
import { AlterarSenhaComponent } from '../alterar-senha/alterar-senha.component';
import { InscricoesComponent } from '../inscricoes/inscricoes.component';
import { Afiliado } from 'src/app/shared/models/afiliado';
import { AfiliadoService } from 'src/app/core/Afiliado/afiliado.service';
import { AfiliadoComponent } from '../afiliado/afiliado.component';
import { AfiliadosEventoComponent } from '../afiliados-evento/afiliados-evento.component';
import { InscricoesAtletaComponent } from '../inscricoes-atleta/inscricoes-atleta.component';
import { PixService } from 'src/app/core/Pix/pix.service';
import { LoadingComponent } from 'src/app/shared/loading/loading.component';

@Component({
  selector: 'app-painel',
  templateUrl: './painel.component.html',
  styleUrls: ['./painel.component.css'],
})
export class EventsManagerComponent implements OnInit {
  constructor(
    private eventoService: EventosService,
    private atletaService: AtletasService,
    private afiliadoService: AfiliadoService,
    private securityService: SecurityService,
    private pixService: PixService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  @ViewChild('tabgroup') tabGroup: MatTabGroup;
  @ViewChild('paginatorAtleta') paginatorAtleta: MatPaginator;
  @ViewChild('paginatorAfiliado') paginatorAfiliado: MatPaginator;
  @ViewChild('paginatorEvento') paginatorEvento: MatPaginator;
  @ViewChild('imageUpload') imageUpload: ImageUploadComponent;
  @ViewChild('categoriesComponent') categoriesComponent: CategoriesComponent;

  userInfo: any;
  eventos: any[] = [];
  dsEventos: MatTableDataSource<any>;
  atletas: Atleta[] = [];
  dsAtletas: MatTableDataSource<Atleta>;
  afiliados: Afiliado[] = [];
  dsAfiliados: MatTableDataSource<Afiliado>;
  atleta: Atleta;
  evento: Evento;

  loadingAtletas: boolean = true;
  loadingEventos: boolean = true;
  loadingPagamentos: boolean = false;

  displayedColumnsEvents: string[] = [
    'status',
    'deletar',
    'editar',
    'nome',
    'data',
    'qtdInscritos',
    'qtdPagos',
    'opções',
  ];
  displayedColumnsAthletes: string[] = [
    'deletar',
    'editar',
    'medico',
    'cpf',
    'nome',
    'opções',
  ];
  displayedColumnsAfiliados: string[] = [
    'deletar',
    'editar',
    'cpf',
    'nome',
    'porcentagem',
    'opções',
  ];
  token: string;

  ngOnInit(): void {
    this.verificarToken();

    if (this.userInfo == null || this.userInfo == undefined) {
      this.dialog
        .open(LoginComponent)
        .afterClosed()
        .subscribe(() => {
          this.verificarToken();
          this.abrirMensagem(
            `Seja bem-vindo ao painel administrativo ${this.userInfo.Name}`
          );
          this.carregarEventos();
          this.carregarAtletas();
          this.carregarAfiliados();
        });
    } else if (this.userInfo?.employee == undefined) {
      this.securityService.logOutToken();
    } else {
      this.carregarEventos();
      this.carregarAtletas();
      this.carregarAfiliados();
    }
  }

  private verificarToken() {
    this.token = this.securityService.getToken();

    if (this.token != '') {
      this.userInfo = this.securityService.getDecodedAccessToken(this.token);
    }
  }

  abrirInscricoes(idEvento: number) {
    const dialogRef = this.dialog.open(InscricoesComponent, {
      data: { idEvento: idEvento },
    });
  }

  novoEvento() {
    this.dialog.open(EventoComponent);
  }

  abrirAfiliados(evento: Evento) {
    this.dialog.open(AfiliadosEventoComponent, {
      data: { evento: evento },
    });
  }

  validarPagamentoxPixNaoConfirmados(idEvento: number) {
    this.loadingPagamentos = true;

    const dialogRef = this.dialog.open(LoadingComponent, {
      data: {
        mensagem:
          'Por favor, aguarde enquanto confirmamos os pagamentos Pix...',
      },
      disableClose: true,
    });

    this.pixService.validarPixNaoProcessados(idEvento).subscribe(
      (retorno) => {
        this.loadingPagamentos = false;

        dialogRef.close();

        this.abrirMensagem(retorno.message);
      },
      (error) => {
        this.loadingPagamentos = false;

        dialogRef.close();

        this.abrirMensagem('Ocorreu um erro ao confirmar os pagamentos.');
      }
    );
  }

  private salvarImagem(eventoId: number) {
    this.eventoService
      .cadastrarImagem(eventoId, this.imageUpload.selectedFile.file)
      .subscribe();
  }

  carregarEventos() {
    this.loadingEventos = true;
    this.eventoService.getAllEvents().subscribe(
      (events) => {
        this.eventos = events;
        this.dsEventos = new MatTableDataSource(this.eventos);
        this.dsEventos.paginator = this.paginatorEvento;

        this.loadingEventos = false;
      },
      (error) => {
        if (error.status == 401) {
          this.securityService.logOutToken();
        }

        this.loadingEventos = false;
      }
    );
  }

  private carregarAtletas() {
    this.loadingAtletas = true;

    this.atletaService.consultarTodosAtletas().subscribe(
      (athletes) => {
        this.atletas = athletes;
        this.dsAtletas = new MatTableDataSource(this.atletas);
        this.dsAtletas.paginator = this.paginatorAtleta;

        this.loadingAtletas = false;
      },
      (error) => {
        if (error.status == 401) {
          this.securityService.logOutToken();
        }

        this.loadingAtletas = false;
      }
    );
  }

  private carregarAfiliados() {
    this.afiliadoService.consultarTodosAfiliados().subscribe(
      (afiliados) => {
        this.afiliados = afiliados;
        this.dsAfiliados = new MatTableDataSource(this.afiliados);
        this.dsAfiliados.paginator = this.paginatorAfiliado;
      },
      (error) => {
        if (error.status == 401) {
          this.securityService.logOutToken();
        } else if (error.status == 404) {
          this.afiliados = [];
          this.dsAfiliados = new MatTableDataSource(this.afiliados);
          this.dsAfiliados.paginator = this.paginatorAfiliado;
        }
      }
    );
  }

  abrirCadastroAtleta(atleta: Atleta) {
    const dialogRef = this.dialog.open(AtletaComponent, {
      data: { atleta: atleta },
    });

    dialogRef.afterClosed().subscribe((result) => {
      this.carregarAtletas();
    });
  }

  abrirCadastroAfiliado(afiliado: Afiliado) {
    const dialogRef = this.dialog
      .open(AfiliadoComponent, {
        data: { afiliado: afiliado },
      })
      .afterClosed()
      .subscribe(() => {
        this.carregarAfiliados();
      });
  }

  alterarSenhaAtleta(atleta: Atleta) {
    this.dialog.open(AlterarSenhaComponent, {
      data: { idAtleta: atleta.id! },
    });
  }

  inscricoesAtleta(atleta: Atleta) {
    this.dialog.open(InscricoesAtletaComponent, {
      data: { cpfAtleta: atleta.cpf },
    });
  }

  cadastrarNovoAtleta() {
    this.dialog.open(AtletaComponent);
  }

  cadastrarNovoAfiliado() {
    this.dialog
      .open(AfiliadoComponent)
      .afterClosed()
      .subscribe(() => {
        this.carregarAfiliados();
      });
  }

  carregarDadosMedicos(atletaId: string) {
    this.dialog.open(DadosMedicosComponent, {
      data: { atletaId: atletaId },
    });
  }

  abrirComponenteCortesia(evento: Evento) {
    this.dialog.open(CortesiaComponent, {
      data: { evento: evento },
    });
  }

  abrirComponenteRelatorio(evento: Evento) {
    this.dialog.open(RelatoriosComponent, { data: { evento: evento } });
  }

  abrirComponenteCupom(evento: Evento) {
    this.dialog.open(CuponsComponent, { data: { evento: evento } });
  }

  editarEvento(evento: Evento) {
    this.dialog.open(EventoComponent, {
      data: {
        eventoId: evento.id!,
      },
    });
  }

  deletarEvento(eventId: number) {
    this.dialog
      .open(MensagemConfirmacaoComponent, {
        data: { mensagem: 'Confirma a exclusão deste Evento?' },
      })
      .afterClosed()
      .subscribe((confirma) => {
        if (confirma)
          this.eventoService.deletarEventoPeloId(eventId).subscribe(() => {
            this.abrirMensagem('Evento excluído com sucesso.');
            this.carregarEventos();
          });
      });
  }

  deletarAtleta(atletaId: string) {
    this.dialog
      .open(MensagemConfirmacaoComponent, {
        data: { mensagem: 'Confirma a exclusão deste Atleta?' },
      })
      .afterClosed()
      .subscribe((confirma) => {
        if (confirma)
          this.atletaService.deletarAtleta(atletaId).subscribe(() => {
            this.carregarAtletas();
            this.abrirMensagem('Atleta excluído com sucesso.');
          });
      });
  }

  deletarAfiliado(afiliadoId: string) {
    this.dialog
      .open(MensagemConfirmacaoComponent, {
        data: { mensagem: 'Confirma a exclusão deste Afiliado?' },
      })
      .afterClosed()
      .subscribe((confirma) => {
        if (confirma)
          this.afiliadoService.deletarAfiliado(afiliadoId).subscribe(() => {
            this.carregarAfiliados();
            this.abrirMensagem('Afiliado excluído com sucesso.');
          });
      });
  }

  carregarCategoria(idEvento: number, nomeEvento: string, percursos: string) {
    this.dialog.open(CategoriesComponent, {
      data: {
        idEvento: idEvento,
        nomeEvento: nomeEvento,
        percursos: percursos,
      },
    });
  }

  carregarRegulamento(eventoId: number, nomeEvento: string) {
    this.dialog.open(RegulamentosComponent, {
      data: { eventoId: eventoId, nomeEvento: nomeEvento },
    });
  }

  aplicarFiltroAtletas(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dsAtletas.filter = filterValue.trim().toLowerCase();
  }

  aplicarFiltroEventos(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dsEventos.filter = filterValue.trim().toLowerCase();
  }

  cadastrarAtletaEvento(evento: Evento) {
    this.dialog.open(CadastroInscricaoComponent, {
      data: {
        evento: evento,
      },
    });
  }

  efetivarAtletaInscricao(evento: Evento) {
    this.dialog.open(EfetivarInscricaoComponent, {
      data: {
        evento: evento,
      },
    });
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 5000,
    });
  }

  isDataFimInscricaoPassada(dataFimInscricao: string): boolean {
    // Converta a string para um objeto Date
    const dataFimInscricaoDate = new Date(dataFimInscricao);

    // Obtenha a data atual
    const dataAtual = new Date();

    // Compare as datas
    return dataFimInscricaoDate < dataAtual;
  }
}
