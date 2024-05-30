import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { CategoriesService } from 'src/app/core/Categorias/categories-service.service';
import { Categoria } from 'src/app/shared/models/categoria';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MensagemConfirmacaoComponent } from 'src/app/shared/mensagem-confirmacao/mensagem-confirmacao.component';

export interface DialogData {
  idEvento: number;
  nomeEvento: string;
  percursos: string;
}

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css'],
})
export class CategoriesComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private categoriaService: CategoriesService,
    private securityService: SecurityService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  @ViewChild('paginatorCategoria') paginatorCategoria: MatPaginator;

  categorieList: string[] = [];
  nomeEvento: string;
  painelConsulta = true;
  categorias: Categoria[] = [];
  dsCategorias: MatTableDataSource<Categoria>;
  displayedColumnsCategorias: string[] = [
    'deletar',
    'editar',
    'categoria',
    'nome',
  ];
  categoriaControl = this.formBuilder.group({
    id: new FormControl(''),
    descSubcategoria: new FormControl('', Validators.required),
    categoria: new FormControl(0, Validators.required),
    filtro_sexo: new FormControl(0, Validators.required),
    filtro_dupla: new FormControl(0, Validators.required),
    idade_de: new FormControl('', Validators.required),
    idade_ate: new FormControl('', Validators.required),
    aviso: new FormControl(''),
    ativo: new FormControl(1, Validators.required),
  });

  ngOnInit(): void {
    this.nomeEvento = this.data.nomeEvento;
    const categories = this.data.percursos.split(';');

    categories.forEach((item) => {
      this.categorieList.push(item);
    });
    this.carregaCategorias(this.data.idEvento, this.data.percursos);
  }

  public carregaCategorias(eventoId: number, percursos: string) {
    this.categoriaService.consultarCategoriasDoEvento(eventoId).subscribe(
      (categorias) => {
        this.categorias = categorias;
        categorias.forEach((categoria) => {
          categoria.percurso = percursos
            .split(';')
            .filter((p, index) => index + 1 == categoria.categoria)[0];
        });
        this.dsCategorias = new MatTableDataSource(this.categorias);
        this.dsCategorias.paginator = this.paginatorCategoria;
      },
      (error) => {
        if (error.status == 401) {
          this.securityService.logOutToken();
        }
      }
    );
  }

  salvarCategoria() {
    const categoria: Categoria = {
      idEvento: this.data.idEvento,
      categoria: Number(this.categoriaControl.get('categoria')?.value!),
      descSubcategoria: this.categoriaControl.get('descSubcategoria')?.value!,
      filtroSexo: Number(this.categoriaControl.get('filtro_sexo')?.value!),
      filtroDupla: Number(this.categoriaControl.get('filtro_dupla')?.value!),
      idadeDe: Number(this.categoriaControl.get('idade_de')?.value!),
      idadeAte: Number(this.categoriaControl.get('idade_ate')?.value!),
      aviso: this.categoriaControl.get('aviso')?.value
        ? this.categoriaControl.get('aviso')?.value!
        : '',
      ativo: Boolean(this.categoriaControl.get('ativo')?.value!),
    };

    if (this.categoriaControl.get('id')?.value) {
      categoria.id = Number(this.categoriaControl.get('id')!.value!);
      this.categoriaService.editarCategoria(categoria).subscribe(() => {
        this.voltarPainelConsulta();
        this.abrirMensagem(`Categoria atualizada com sucesso`);
      });
    } else {
      this.categoriaService.cadastrarCategoria(categoria).subscribe(() => {
        this.voltarPainelConsulta();

        this.abrirMensagem(`Categoria cadastrada com sucesso`);
      });
    }
  }

  editarCategoria(idCategoria: number) {
    this.limparCategoriaControl();

    this.categoriaService
      .consultarCategoriaId(idCategoria)
      .subscribe((categoria) => {
        this.categoriaControl.get('id')?.setValue(categoria.id!.toString());
        this.categoriaControl.get('categoria')?.setValue(categoria.categoria);
        this.categoriaControl
          .get('descSubcategoria')
          ?.setValue(categoria.descSubcategoria);
        this.categoriaControl
          .get('filtro_sexo')
          ?.setValue(categoria.filtroSexo);
        this.categoriaControl
          .get('filtro_dupla')
          ?.setValue(categoria.filtroDupla);
        this.categoriaControl
          .get('idade_de')
          ?.setValue(categoria.idadeDe.toString());
        this.categoriaControl
          .get('idade_ate')
          ?.setValue(categoria.idadeAte.toString());
        this.categoriaControl.get('aviso')?.setValue(categoria.aviso);
        this.categoriaControl.get('ativo')?.setValue(Number(categoria.ativo));
      });

    this.painelConsulta = false;
  }

  deletarCategoria(categoriaId: number) {
    this.dialog
      .open(MensagemConfirmacaoComponent, {
        data: { mensagem: 'Confirma a exclusão desta Categoria?' },
      })
      .afterClosed()
      .subscribe((confirma) => {
        if (confirma) {
          this.categoriaService.deletarCategoria(categoriaId).subscribe(() => {
            this.abrirMensagem('Categoria excluída com sucesso');
            this.carregaCategorias(this.data.idEvento, this.data.percursos);
          });
        }
      });
  }

  protected novaCategoria() {
    this.categoriaControl.reset();
    this.painelConsulta = false;
  }

  protected voltarPainelConsulta() {
    this.painelConsulta = true;
    this.carregaCategorias(this.data.idEvento, this.data.percursos);
  }

  private limparCategoriaControl() {
    this.categoriaControl.reset();
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
