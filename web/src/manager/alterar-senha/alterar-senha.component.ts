import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { ManagerNovaSenha } from 'src/app/shared/models/Atleta/nova-senha';

export interface DialogData {
  idAtleta: string;
}

@Component({
  selector: 'app-alterar-senha',
  templateUrl: './alterar-senha.component.html',
  styleUrls: ['./alterar-senha.component.css'],
})
export class AlterarSenhaComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private securityService: SecurityService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<AlterarSenhaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  senhaControl = this.formBuilder.group({
    novaSenha: new FormControl('', Validators.required),
    confirmarSenha: new FormControl('', Validators.required),
  });

  alterandoSenha: boolean = false;

  ngOnInit(): void {}

  salvarNovaSenha() {
    if (
      this.senhaControl.get('novaSenha')?.value !=
      this.senhaControl.get('confirmarSenha')?.value
    ) {
      this.abrirMensagem('As senhas não são iguais');
      return;
    }

    var novaSenha: ManagerNovaSenha = {
      id: this.data.idAtleta,
      novaSenha: this.senhaControl.get('novaSenha')?.value!,
    };

    this.securityService.managerAlterarSenha(novaSenha).subscribe(
      () => {
        this.abrirMensagem('Senha atualizada com sucesso');

        this.dialogRef.close();
      },
      (error) => {
        this.abrirMensagem(
          'Ocorreu um erro ao atualizar a nova senha. Contate nossos administradores'
        );
      }
    );
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
