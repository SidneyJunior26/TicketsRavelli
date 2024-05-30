import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AfiliadoService } from 'src/app/core/Afiliado/afiliado.service';
import { Afiliado } from 'src/app/shared/models/afiliado';

export interface DialogData {
  afiliado: Afiliado;
}

@Component({
  selector: 'app-afiliado',
  templateUrl: './afiliado.component.html',
  styleUrls: ['./afiliado.component.css'],
})
export class AfiliadoComponent implements OnInit {
  constructor(
    private afiliadoService: AfiliadoService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<AfiliadoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  novoAfiliado: boolean = true;
  afiliado: Afiliado;

  afiliadoControl = this.formBuilder.group({
    id: new FormControl(''),
    cpf: new FormControl('', [Validators.required, Validators.minLength(11)]),
    nome: new FormControl('', Validators.required),
    porcentagem: new FormControl(5, Validators.required),
  });

  ngOnInit(): void {
    if (this.data != null) {
      this.afiliado = this.data.afiliado;
      this.carregarAfiliadoControl();
      this.novoAfiliado = false;
    } else {
      this.novoAfiliado = true;
    }
  }

  private carregarAfiliadoControl() {
    for (const key in this.afiliado) {
      if (this.afiliadoControl.get(key)) {
        this.afiliadoControl.get(key)?.setValue(this.afiliado[key]);
      }
    }
  }

  protected cadastrarAfiliado() {
    const afiliado: Afiliado = {
      cpf: this.afiliadoControl.get('cpf')!.value!.toString(),
      nome: this.afiliadoControl.get('nome')!.value!.toString(),
      porcentagem: Number(this.afiliadoControl.get('porcentagem')!.value!),
    };

    this.afiliadoService.cadastrarAfiliado(afiliado).subscribe(
      () => {
        this.abrirMensagem('Afiliado cadastrado');
        this.dialogRef.close();
      },
      () => {
        this.abrirMensagem('Erro ao cadastrar afiliado');
      }
    );
  }

  protected atualizarAfliado() {
    const afiliadoId = this.afiliadoControl.get('id')!.value!.toString();

    const afiliado: Afiliado = {
      cpf: this.afiliadoControl.get('cpf')!.value!.toString(),
      nome: this.afiliadoControl.get('nome')!.value!.toString(),
      porcentagem: Number(this.afiliadoControl.get('porcentagem')!.value!),
    };

    this.afiliadoService.atualizarAfiliado(afiliadoId, afiliado).subscribe(
      () => {
        this.abrirMensagem('Afiliado atualizado');
        this.dialogRef.close();
      },
      () => {
        this.abrirMensagem('Erro ao atualizar afiliado');
      }
    );
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
