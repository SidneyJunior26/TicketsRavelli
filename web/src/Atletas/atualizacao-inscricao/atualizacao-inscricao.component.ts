import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CategoriesService } from 'src/app/core/Categorias/categories-service.service';
import { EventosService } from 'src/app/core/Eventos/events.service';
import { SubscriptionsService } from 'src/app/core/Inscricoes/inscricoes.service';
import { SubCategoriaService } from 'src/app/core/SubCategorias/sub-categoria.service';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { Evento } from 'src/app/shared/models/evento';
import { Subscription } from 'src/app/shared/models/inscricao';
import { SubCategoria } from 'src/app/shared/models/subCategoria';

export interface DialogData {
  idInscricao: number;
  atleta: Atleta;
  adm: boolean;
}

@Component({
  selector: 'app-atualizacao-inscricao',
  templateUrl: './atualizacao-inscricao.component.html',
  styleUrls: ['./atualizacao-inscricao.component.css'],
})
export class AtualizacaoInscricaoComponent implements OnInit {
  constructor(
    private inscricoesService: SubscriptionsService,
    private eventoService: EventosService,
    private formBuilder: FormBuilder,
    private subCategoriaService: SubCategoriaService,
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

  inscricao: Subscription;
  evento: Evento;
  subCategoria: SubCategoria;

  idEvento: number;
  selecionado: boolean = true;
  filtroDupla: number = 0;

  lstCategoria: string[] = [];
  subCategorieList: [
    { id: string; name: string; aviso: string; filtroDupla: number }
  ] = [{ id: '', name: '', aviso: '', filtroDupla: 0 }];
  categoriaSelecionada: { id: number; descricao: string };

  ngOnInit(): void {
    this.consultarInscricao();
  }

  private consultarInscricao() {
    this.inscricoesService
      .getSubscriptionById(this.data.idInscricao)
      .subscribe((retorno) => {
        this.inscricao = retorno;

        this.consultarEvento();
      });
  }

  private consultarEvento() {
    this.eventoService
      .consultarEventoPeloId(this.inscricao.idEvento)
      .subscribe((evento) => {
        this.evento = evento;

        this.carregarCategorias();
      });
  }

  private carregarCategorias() {
    const categorias = this.evento.categoria.split(';');

    categorias.forEach((item) => {
      this.lstCategoria.push(item);
    });

    this.consultarSubCategoria();
  }

  consultarSubCategoria() {
    this.subCategoriaService
      .consultarSubCategoria(this.inscricao.idSubcategoria)
      .subscribe((subcategoria) => {
        this.subCategoria = subcategoria;

        if (
          this.subCategoria.categoria.toString() ==
            this.inscricaoControl.get('categoria')?.value!.toString() ||
          this.inscricaoControl.get('categoria')?.value!.toString() == ''
        ) {
          this.inscricaoControl
            .get('categoria')
            ?.setValue(this.subCategoria.categoria.toString());
          this.inscricaoControl
            .get('subCategoria')
            ?.setValue(this.subCategoria.id!.toString());
          this.filtroDupla = this.subCategoria.filtroDupla;
        } else {
          this.inscricaoControl.get('subCategoria')?.reset();
        }

        if (this.data.adm) {
          this.carregarSubCategoriasAdm();
        } else {
          this.carregarSubCategoriasFiltrado();
        }
      });
  }

  carregarFiltroDupla() {
    var subCategoriaSelecionada = this.subCategorieList.find(
      (x) => x.id == this.inscricaoControl.get('subCategoria')?.value
    );

    this.filtroDupla = subCategoriaSelecionada?.filtroDupla!;
  }

  carregarSubCategoriasFiltrado() {
    let category = Number(this.inscricaoControl.get('categoria')!.value!);
    let sexo = this.data.atleta.sexo;
    let nascimento = new Date(this.data.atleta.nascimento).getFullYear();
    let dtNascimento = new Date().getFullYear();

    if (nascimento != null) {
      dtNascimento = new Date(nascimento).getFullYear();
    }

    let dataEvento = new Date(this.inscricao.evento!.data).getFullYear();
    //let idade = moment().diff(nascimento, 'years');
    let idade = dataEvento - nascimento;

    this.categoriaService
      .consultarCategoriasFiltrado(
        this.evento.id!,
        category,
        idade,
        Number(sexo)
      )
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
      });

    this.carregarFormControl();
  }

  carregarSubCategoriasAdm() {
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
      });

    this.carregarFormControl();
  }

  preCarregarCategoria(id: number, descricao: string) {
    if (this.selecionado) {
      this.categoriaSelecionada = {
        id: Number(this.inscricaoControl.get('subCategoria')?.value),
        descricao: descricao,
      };

      this.consultarSubCategoria();

      this.selecionado = false;
    } else this.selecionado = true;
  }

  private carregarFormControl() {
    this.inscricaoControl
      .get('equipe')
      ?.setValue(this.inscricao.equipe ? this.inscricao.equipe : '');

    this.inscricaoControl
      .get('nomeDupla')
      ?.setValue(this.inscricao.dupla ? this.inscricao.dupla : '');

    this.inscricaoControl
      .get('nomeTrioQuarteto')
      ?.setValue(this.inscricao.quarteto ? this.inscricao.quarteto : '');
  }

  protected atualizarInscricao() {
    this.inscricao.equipe = this.inscricaoControl.get('equipe')?.value!;
    this.inscricao.dupla = this.inscricaoControl.get('nomeDupla')?.value
      ? this.inscricaoControl.get('nomeDupla')?.value!
      : '';
    this.inscricao.quarteto = this.inscricaoControl.get('nomeTrioQuarteto')
      ?.value
      ? this.inscricaoControl.get('nomeTrioQuarteto')?.value!
      : '';

    this.inscricao.idSubcategoria = Number(
      this.inscricaoControl.get('subCategoria')?.value
    );

    this.inscricoesService.putSubscription(this.inscricao).subscribe(
      () => {
        this.abrirMensagem('Inscrição atualizada');
      },
      (error) => {
        this.abrirMensagem('Ocorreu um erro ao atualizar a inscrição');
      }
    );
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
