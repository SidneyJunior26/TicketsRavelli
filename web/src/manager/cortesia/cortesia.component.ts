import { SelectionModel } from '@angular/cdk/collections';
import { Component, Inject, Input, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { AtletasService } from 'src/app/core/Atletas/atletas.service';
import { CortesiaService } from 'src/app/core/Cortesia/cortesia.service';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { Evento } from 'src/app/shared/models/evento';

export interface DialogData {
  evento: Evento;
}

export interface AtletasTable {
  atletas: Atleta;
  index: number;
}

@Component({
  selector: 'app-cortesia',
  templateUrl: './cortesia.component.html',
  styleUrls: ['./cortesia.component.css'],
})
export class CortesiaComponent implements OnInit {
  @ViewChild('paginatorAtleta') paginatorCortesia: MatPaginator;

  emailAtleta: string;
  nomeAtleta: string;

  constructor(
    private cortesiaServide: CortesiaService,
    private atletaService: AtletasService,
    private securityService: SecurityService,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  costesias: any[] = [];
  dsCortesias: MatTableDataSource<any>;
  selection = new SelectionModel<Atleta>(true, []);
  itensSelecionados: any[] = [];

  loading: boolean = true;
  gerandoCupom: boolean = false;

  displayedColumnsCortesias: string[] = ['deletar', 'cupom', 'status'];

  ngOnInit(): void {
    this.carregarCuponsEvento();
  }

  private carregarCuponsEvento() {
    this.cortesiaServide
      .consultarCupomCortesiaEvento(this.data.evento.id!)
      .subscribe(
        (cortesias) => {
          this.costesias = cortesias;
          this.dsCortesias = new MatTableDataSource(this.costesias);
          this.dsCortesias.paginator = this.paginatorCortesia;
        },
        (error) => {
          if (error.status == 401) {
            this.securityService.logOutToken();
          }
        },
        () => (this.loading = false)
      );
  }

  aplicarFiltroCortesias(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dsCortesias.filter = filterValue.trim().toLowerCase();
  }

  gerarCupom() {
    this.gerandoCupom = true;
    this.cortesiaServide.cadastrarCupomCortesia(this.data.evento).subscribe(
      () => {
        this.abrirMensagem('Novo cupom gerado com sucesso');
        this.carregarCuponsEvento();
      },
      (error) => {
        this.abrirMensagem('Ocorreu um erro ao gerar o cupom');
      },
      () => {
        this.gerandoCupom = false;
      }
    );
  }

  deletarCupom(cupom: string) {
    this.cortesiaServide.deletarCupom(cupom).subscribe(
      () => {
        this.abrirMensagem('Cupom apagado');

        this.carregarCuponsEvento();
      },
      () => {
        this.abrirMensagem('Erro ao tentar remover o cupom');
      }
    );
  }

  alterarStatusCupom(cupom: string) {
    this.cortesiaServide.alterarStatus(cupom).subscribe(
      () => {
        this.abrirMensagem('Alterado status do cupom');

        this.carregarCuponsEvento();
      },
      () => {
        this.abrirMensagem('Erro ao alterar o status do cupom');
      }
    );
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
