import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RegulamentoService } from 'src/app/core/Regulamento/regulatmento.service';
import { Regulamento } from 'src/app/shared/models/regulamento';

export interface DialogData {
  eventoId: number;
  nomeEvento: string;
}

@Component({
  selector: 'app-regulamentos',
  templateUrl: './regulamentos.component.html',
  styleUrls: ['./regulamentos.component.css'],
})
export class RegulamentosComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private regulamentoService: RegulamentoService,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  nomeEvento: string;
  regulamento: Regulamento;
  regulamentoControl = this.formBuilder.group({
    regulamento: new FormControl('', Validators.required),
    termos: new FormControl('', Validators.required),
  });
  novoRegulamento: boolean;

  ngOnInit(): void {
    this.nomeEvento = this.data.nomeEvento;
    this.regulamentoService
      .consultarRegulamentoPorEvento(this.data.eventoId)
      .subscribe(
        (regulamento) => {
          this.regulamento = regulamento;
          this.novoRegulamento = false;
          this.regulamentoControl
            .get('regulamento')
            ?.setValue(regulamento.regulamento1);
          this.regulamentoControl
            .get('termos')
            ?.setValue(regulamento.compromisso);
        },
        (error) => {
          if (error.status == 404) {
            this.novoRegulamento = true;
          }
        }
      );
  }

  salvarRegulamento() {
    const regulamento: Regulamento = {
      idEvento: this.data.eventoId,
      regulamento1: this.regulamentoControl.get('regulamento')?.value!,
      compromisso: this.regulamentoControl.get('termos')?.value!,
    };

    if (this.novoRegulamento) {
      this.regulamentoService
        .cadastrarRegulamento(regulamento)
        .subscribe(() => {
          this.abrirMensagem('Regulamento salvo');
        });
    } else {
      this.regulamentoService
        .atualizarRegulamento(regulamento)
        .subscribe(() => {
          this.abrirMensagem('Regulamento salvo');
        });
    }
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
