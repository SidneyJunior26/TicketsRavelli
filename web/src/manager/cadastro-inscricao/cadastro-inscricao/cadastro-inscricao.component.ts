import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CategoriesService } from 'src/app/core/Categorias/categories-service.service';
import { SubscriptionsService } from 'src/app/core/Inscricoes/inscricoes.service';
import { Subscription } from 'src/app/shared/models/inscricao';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Evento } from 'src/app/shared/models/evento';
import { SubCategoria } from 'src/app/shared/models/subCategoria';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { AtletasService as AtletasService } from 'src/app/core/Atletas/atletas.service';
import * as moment from 'moment';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import { MatPaginator } from '@angular/material/paginator';

export interface DialogData {
  evento: Evento;
}

@Component({
  selector: 'app-cadastro-inscricao',
  templateUrl: './cadastro-inscricao.component.html',
  styleUrls: ['./cadastro-inscricao.component.css'],
})
export class CadastroInscricaoComponent implements OnInit {
  constructor(
    private inscricoesService: SubscriptionsService,
    private atletaService: AtletasService,
    private formBuilder: FormBuilder,
    private categoriaService: CategoriesService,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  inscricaoControl = this.formBuilder.group({
    categoria: new FormControl('', Validators.required),
    subCategoria: new FormControl('', Validators.required),
    equipe: new FormControl(''),
    nomeDupla: new FormControl(''),
    nomeTrioQuarteto: new FormControl(''),
  });

  displayedColumnsAtletas: string[] = ['select', 'cpf', 'nome', 'email'];

  dsAtletas: MatTableDataSource<Atleta>;
  selection = new SelectionModel<Atleta>(true, []);
  itensSelecionados: any[] = [];
  @ViewChild('paginatorAtleta') paginatorAtleta: MatPaginator;

  inscricao: Subscription;
  evento: Evento;
  subCategoria: SubCategoria;

  idEvento: number;
  selecionado: boolean = true;

  loading: boolean = true;
  cadastrando: boolean = false;

  atletas: Atleta[] = [];
  lstCategoria: string[] = [];
  subCategorieList: [
    { id: string; name: string; aviso: string; filtroDupla: number }
  ] = [{ id: '', name: '', aviso: '', filtroDupla: 0 }];
  categoriaSelecionada: { id: number; descricao: string };

  isItemSelected: boolean = false;

  ngOnInit(): void {
    this.evento = this.data.evento;

    this.consultarAtletas();
  }

  private consultarAtletas() {
    this.atletaService.consultarTodosAtletas().subscribe(
      (atletas) => {
        this.atletas = atletas;

        this.dsAtletas = new MatTableDataSource(this.atletas);
        this.dsAtletas.paginator = this.paginatorAtleta;
      },
      (error) => {
        this.abrirMensagem('Ocorreu um erro ao carregar os atletas');
      },
      () => this.carregarCategorias()
    );
  }

  private carregarCategorias() {
    const categorias = this.evento.categoria.split(';');

    categorias.forEach((item) => {
      this.lstCategoria.push(item);
    });

    this.consultarSubCategoria();
  }

  consultarSubCategoria() {
    let atleta = this.atletas.find(
      (atleta) => atleta.id == this.inscricaoControl.get('idAtleta')?.value
    );

    let sexo = atleta?.sexo;
    let nascimento = atleta?.nascimento;
    let dtNascimento = new Date().getFullYear();

    if (nascimento != null && nascimento != '') {
      dtNascimento = new Date(nascimento).getFullYear();
    }

    let idade = moment().diff(nascimento, 'years');

    this.categoriaService
      .consultarCategoriasDoEvento(this.evento.id!)
      .subscribe((subCategories) => {
        this.subCategorieList = [
          { id: '', name: '', aviso: '', filtroDupla: 0 },
        ];

        subCategories.forEach((item) => {
          this.subCategorieList.push({
            id: item.id!.toString(),
            name: item.descSubcategoria,
            aviso: item.aviso,
            filtroDupla: item.filtroDupla,
          });
        });

        this.subCategorieList.splice(0, 1);

        this.loading = false;
      });
    this.selecionado = false;
  }

  preCarregarCategoria(id: number, descricao: string) {
    if (this.selecionado) {
      this.categoriaSelecionada = {
        id: Number(this.inscricaoControl.get('subCategoria')?.value),
        descricao: descricao,
      };

      this.consultarSubCategoria();
    } else this.selecionado = true;
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }

  aplicarFiltroAtletas(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dsAtletas.filter = filterValue.trim().toLowerCase();
  }

  cadastrar() {
    this.cadastrando = true;
    this.itensSelecionados = this.selection.selected;

    const cpfSelecionados = this.itensSelecionados.map((item) => item.cpf);

    cpfSelecionados.forEach((cpf) => {
      const novaInscricao: Subscription = {
        cpfAtleta: cpf,
        idEvento: this.data.evento.id!,
        idSubcategoria: Number(
          this.inscricaoControl.get('subCategoria')?.value
        ),
        equipe: this.inscricaoControl.get('equipe')?.value!,
        dupla: this.inscricaoControl.get('nomeDupla')?.value!,
        quarteto: this.inscricaoControl.get('nomeTrioQuarteto')?.value!,
        aceiteRegulamento: true,
        pago: false,
      };

      this.inscricoesService.postSubscription(novaInscricao).subscribe(
        () => {
          this.abrirMensagem('Inscrição realizada com sucesso');

          this.cadastrando = false;
        },
        (error) => {
          this.abrirMensagem('Ocorreu um erro ao realizar a inscrição');

          this.cadastrando = false;
        }
      );
    });

    cpfSelecionados.forEach((cpf) => {});
  }
}
