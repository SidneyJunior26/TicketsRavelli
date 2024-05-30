import { SelectionModel } from '@angular/cdk/collections';
import {
  Component,
  ElementRef,
  Inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { SubscriptionsService } from 'src/app/core/Inscricoes/inscricoes.service';
import { Evento } from 'src/app/shared/models/evento';
import { Subscription } from 'src/app/shared/models/inscricao';

export interface DialogData {
  evento: Evento;
  nomeAtleta?: string;
}

@Component({
  selector: 'app-efetivar-inscricao',
  templateUrl: './efetivar-inscricao.component.html',
  styleUrls: ['./efetivar-inscricao.component.css'],
})
export class EfetivarInscricaoComponent implements OnInit {
  inscricoes: Subscription[] = [];
  dsInscricoes: MatTableDataSource<Subscription>;
  displayedColumnsInscricoes: string[] = ['select', 'cpf', 'nome', 'pacote'];
  selection = new SelectionModel<Subscription>(true, []);
  @ViewChild('paginatorInscricoes') paginatorInscricoes: MatPaginator;
  @ViewChild('input', { static: false }) inputFiltro: ElementRef;

  loadingInscricoes: boolean = true;

  efetivandoInscricao: boolean = false;

  constructor(
    private inscricaoService: SubscriptionsService,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  efetivarControl = this.formBuilder.group({
    valor: new FormControl('', Validators.required),
    metodoPagamento: new FormControl(1, Validators.required),
  });

  ngOnInit(): void {
    this.consultarInscricoesNaoEfetivadas();
  }

  private consultarInscricoesNaoEfetivadas() {
    this.inscricaoService
      .getNonEffectiveSubscriptionsByEvent(this.data.evento.id!)
      .subscribe(
        (inscritos) => {
          this.dsInscricoes = new MatTableDataSource(inscritos);
          this.dsInscricoes.paginator = this.paginatorInscricoes;

          this.inscricoes = inscritos;
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
            return data.atleta!.nome.toLocaleLowerCase().includes(filter);
          };

          if (this.data.nomeAtleta != null && this.data.nomeAtleta != '') {
            this.inputFiltro.nativeElement.value = this.data.nomeAtleta;

            this.aplicarFiltroInscricoes();
          }
        }
      );
  }

  protected efetivarAtleta(idInscricao: number) {
    this.efetivandoInscricao = true;

    let efetivarAtleta = {
      valorPago: this.efetivarControl.get('valor')?.value!.toString(),
      metodoPagamento: Number(
        this.efetivarControl.get('metodoPagamento')?.value!
      ),
    };

    console.log(efetivarAtleta);

    this.inscricaoService
      .effectSubscription(idInscricao, efetivarAtleta)
      .subscribe(
        () => {
          this.abrirMensagem('Atleta Efetivado');

          this.efetivarControl.get('valor')?.setValue('');

          this.efetivandoInscricao = false;
        },
        (error) => {
          this.abrirMensagem('Ocorreu um erro ao efetivar o atleta');

          this.efetivandoInscricao = false;
        }
      );
  }

  aplicarFiltroInscricoes() {
    var filterValue = this.inputFiltro.nativeElement.value.toString();
    this.dsInscricoes.filter = filterValue.trim().toLowerCase();
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
