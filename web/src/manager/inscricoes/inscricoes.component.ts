import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { AtualizacaoInscricaoComponent } from 'src/app/Atletas/atualizacao-inscricao/atualizacao-inscricao.component';
import { SubscriptionsService } from 'src/app/core/Inscricoes/inscricoes.service';
import { MensagemConfirmacaoComponent } from 'src/app/shared/mensagem-confirmacao/mensagem-confirmacao.component';
import { Subscription } from 'src/app/shared/models/inscricao';
import { EfetivarInscricaoComponent } from '../efetivar-inscricao/efetivar-inscricao.component';
import { Evento } from 'src/app/shared/models/evento';

export interface DialogData {
  idEvento: number;
}

@Component({
  selector: 'app-inscricoes',
  templateUrl: './inscricoes.component.html',
  styleUrls: ['./inscricoes.component.css'],
})
export class InscricoesComponent implements OnInit {
  @ViewChild('paginatorInscricoes') paginatorInscricoes: MatPaginator;

  inscricoes: Subscription[] = [];
  dsInscricoes: MatTableDataSource<Subscription>;
  displayedColumnsInscricoes: string[] = [
    'excluir',
    'editar',
    'efetivar',
    'cpf',
    'nome',
    'categoria',
    'dtInscricao',
    'tipoPagamento',
    'vlrPago',
    'pago',
  ];

  loadingInscricoes: boolean = true;

  constructor(
    private inscricoesService: SubscriptionsService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  ngOnInit(): void {
    this.consultarInscricoes();
  }

  private consultarInscricoes() {
    this.loadingInscricoes = true;

    this.inscricoesService
      .getSubscriptionsByEvent(this.data.idEvento)
      .subscribe(
        (inscritos) => {
          this.inscricoes = inscritos;

          this.dsInscricoes = new MatTableDataSource(inscritos);
          this.dsInscricoes.paginator = this.paginatorInscricoes;

          this.loadingInscricoes = false;
        },
        (error) => {
          this.abrirMensagem('Ocorreu um erro ao carregar as inscricoes');
        },
        () => {
          this.loadingInscricoes = false;

          this.dsInscricoes.filterPredicate = (
            data: Subscription,
            filter: string
          ) => {
            const lowerCaseFilter = filter.toLocaleLowerCase();

            return (
              data.atleta!.nome.toLocaleLowerCase().includes(lowerCaseFilter) ||
              data.cpfAtleta.toLocaleLowerCase().includes(lowerCaseFilter)
            );
          };
        }
      );
  }

  editarInscricao(inscricao: Subscription) {
    this.dialog
      .open(AtualizacaoInscricaoComponent, {
        data: {
          idInscricao: inscricao.id,
          atleta: inscricao.atleta,
          adm: true,
        },
      })
      .afterClosed()
      .subscribe(() => {
        this.consultarInscricoes();
      });
  }

  deletarInscricao(inscricao: Subscription) {
    this.dialog
      .open(MensagemConfirmacaoComponent, {
        data: {
          mensagem:
            'Confirma a exclusão da inscrição do atleta ' +
            inscricao.atleta?.nome +
            '?',
        },
      })
      .afterClosed()
      .subscribe((confirma) => {
        if (confirma)
          this.inscricoesService.deleteSubscription(inscricao.id!).subscribe(
            () => {
              this.abrirMensagem('Inscrição removida');
              this.consultarInscricoes();
            },
            () => {
              this.abrirMensagem('Ocorreu um erro ao remover a inscrição');
            }
          );
      });
  }

  efetivarAtletaInscricao(evento: Evento, nomeAtleta: string) {
    this.dialog
      .open(EfetivarInscricaoComponent, {
        data: {
          evento: evento,
          nomeAtleta: nomeAtleta.trimEnd(),
        },
      })
      .afterClosed()
      .subscribe(() => this.consultarInscricoes());
  }

  aplicarFiltroEventos(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dsInscricoes.filter = filterValue.trim().toLowerCase();
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
