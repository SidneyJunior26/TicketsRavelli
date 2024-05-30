import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { SubscriptionsService } from 'src/app/core/Inscricoes/inscricoes.service';
import { MensagemModalComponent } from 'src/app/shared/mensagem-modal/mensagem-modal.component';
import { Subscription } from 'src/app/shared/models/inscricao';

@Component({
  selector: 'app-inscritos',
  templateUrl: './inscritos.component.html',
  styleUrls: ['./inscritos.component.css'],
})
export class InscritosComponent implements OnInit {
  constructor(
    private inscricoesService: SubscriptionsService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog
  ) {}

  @ViewChild('paginatorInscritos') paginatorInscritos: MatPaginator;

  dsInscritos = new MatTableDataSource<any>();
  inscricoes: Subscription[] = [];
  idEvento: number;

  lstCategorias: string[] = [];
  lstEquipes: string[] = [];

  loading: boolean = true;

  displayedColumnsInscricoes: string[] = [
    'nome',
    'subcategoria.descSubcategoria',
    'equipe',
    'dupla',
  ];

  ngOnInit(): void {
    window.scrollTo(0, 0);
    this.consultarInscricoesEvento();
  }

  aplicarFiltroInscricoes(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dsInscritos.filter = filterValue.trim().toLowerCase();
  }

  aplicarFiltroCategoria(event: any) {
    if (event.value != 'TODAS CATEGORIAS')
      this.dsInscritos.filter = event.value.trim().toLowerCase();
    else {
      this.dsInscritos.filter = '';
      event.value = '';
    }
  }

  aplicarFiltroDupla(event: any) {
    if (event.value != 'TODAS EQUIPES')
      this.dsInscritos.filter = event.value.trim().toLowerCase();
    else {
      this.dsInscritos.filter = '';
      event.value = '';
    }
  }

  voltar() {
    this.router.navigateByUrl('/');
  }

  private consultarInscricoesEvento() {
    this.loading = true;
    this.idEvento = this.activatedRoute.snapshot.params['idEvento'];

    this.inscricoesService.getSubscriptionsByEvent(this.idEvento).subscribe(
      (retorno) => {
        this.inscricoes = retorno;

        this.dsInscritos = new MatTableDataSource(retorno);
        this.dsInscritos.paginator = this.paginatorInscritos;

        const dadosAgrupadosCategorias = this.agruparPorDescSubcategoria(
          this.inscricoes
        );
        const dadosAgrupladosEquipes = this.agruparPorDupla(this.inscricoes);

        this.lstCategorias = Object.keys(dadosAgrupadosCategorias);
        this.lstEquipes = Object.keys(dadosAgrupladosEquipes);

        this.loading = false;
      },
      (error) => {
        if (error.status == 404) {
          this.router.navigateByUrl('/');
          this.dialog.open(MensagemModalComponent, {
            width: '600px',
            data: {
              mensagem: 'Este evento ainda não possui nenhum inscrito\n',
              icone: 'info',
            },
          });
        }

        this.loading = false;
      },
      () => {
        this.dsInscritos.filterPredicate = (data: any, filter: string) => {
          const lowerCaseFilter = filter.toLowerCase();
          return (
            data.atleta.nome.toLowerCase().includes(lowerCaseFilter) ||
            data.subcategoria.descSubcategoria
              .toLowerCase()
              .includes(lowerCaseFilter) ||
            data.equipe.toLowerCase().includes(lowerCaseFilter) ||
            data.dupla.toLowerCase().includes(lowerCaseFilter)
          );
        };
      }
    );
  }

  private agruparPorDescSubcategoria(array: Subscription[]): {
    [key: string]: Subscription[];
  } {
    const agrupamento: { [key: string]: Subscription[] } = {};

    // Adiciona o registro "TODOS"
    agrupamento['TODAS CATEGORIAS'] = [];

    array.forEach((inscricao) => {
      const descSubcategoria =
        inscricao.subcategoria && inscricao.subcategoria.descSubcategoria
          ? inscricao.subcategoria.descSubcategoria
          : 'SemCategoria';

      agrupamento[descSubcategoria] = agrupamento[descSubcategoria] || [];
      agrupamento[descSubcategoria].push(inscricao);

      // Adiciona a inscrição ao grupo "TODOS"
      agrupamento['TODAS CATEGORIAS'].push(inscricao);
    });

    return agrupamento;
  }

  private agruparPorDupla(array: Subscription[]): {
    [key: string]: Subscription[];
  } {
    const agrupamento: { [key: string]: Subscription[] } = {};

    // Adiciona o registro "TODOS"
    agrupamento['TODAS EQUIPES'] = [];

    array.forEach((inscricao) => {
      if (inscricao.equipe) {
        const equipe = inscricao.equipe;

        agrupamento[equipe] = agrupamento[equipe] || [];
        agrupamento[equipe].push(inscricao);

        // Adiciona a inscrição ao grupo "TODOS"
        agrupamento['TODAS EQUIPES'].push(inscricao);
      }
    });

    return agrupamento;
  }
}
