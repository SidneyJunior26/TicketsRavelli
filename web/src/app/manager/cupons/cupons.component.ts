import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { CupomDescontoService } from 'src/app/core/Cupons/cupom.service';
import { Desconto } from 'src/app/shared/models/desconto';
import { Evento } from 'src/app/shared/models/evento';

export interface DialogData {
  evento: Evento;
}

@Component({
  selector: 'app-cupons',
  templateUrl: './cupons.component.html',
  styleUrls: ['./cupons.component.css'],
})
export class CuponsComponent implements OnInit {
  constructor(
    private cupomService: CupomDescontoService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}
  isLoading = true;

  dsDesconto: MatTableDataSource<Desconto>;

  displayedColumnsDesconto: string[] = ['cupom', 'porcentagem', 'ativo'];

  cupomControl = this.formBuilder.group({
    cupom: new FormControl('', Validators.required),
    porcentagem: new FormControl('', Validators.required),
    ativo: new FormControl(true),
  });

  ngOnInit(): void {
    this.consultarCupons();
  }

  private consultarCupons() {
    this.isLoading = true;

    this.cupomService
      .consultatCuponsDescontoEvento(this.data.evento.id!)
      .subscribe(
        (cupons) => {
          this.dsDesconto = new MatTableDataSource(cupons);
          this.isLoading = false;
        },
        () => (this.isLoading = false)
      );
  }

  protected salvarCupom() {
    var desconto: Desconto = {
      idEvento: this.data.evento.id!,
      cupom: this.cupomControl.get('cupom')?.value!,
      porcentagem: Number(this.cupomControl.get('porcentagem')?.value!),
      ativo: Boolean(this.cupomControl.get('ativo')?.value!) ? 1 : 0,
    };

    this.cupomService.cadastrarCupom(desconto).subscribe(() => {
      this.consultarCupons();
      this.abrirMensagem('Cupom salvo com sucesso');
    });
  }

  protected ativarCupom(cupomId: number) {
    this.cupomService.ativarCupom(cupomId).subscribe(
      () => {
        this.consultarCupons();
        this.abrirMensagem('Cupom ativado');
      },
      (error) => {
        this.abrirMensagem(
          'Ocorreu um erro ao ativar o cupom. Verifique o log.'
        );
      }
    );
  }

  protected desativarCupom(cupomId: number) {
    this.cupomService.desativarCupom(cupomId).subscribe(
      () => {
        this.consultarCupons();
        this.abrirMensagem('Cupom desativado');
      },
      (error) => {
        this.abrirMensagem(
          'Ocorreu um erro ao desativar o cupom. Verifique o log.'
        );
      }
    );
  }

  protected aplicarFiltroDesconto(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dsDesconto.filter = filterValue.trim().toLowerCase();
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
