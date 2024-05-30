import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { LoginComponent } from 'src/app/shared/login/login.component';
import { MensagemModalComponent } from 'src/app/shared/mensagem-modal/mensagem-modal.component';
import { EsqueciSenha } from 'src/app/shared/models/Atleta/nova-senha';

@Component({
  selector: 'app-esqueci-senha',
  templateUrl: './esqueci-senha.component.html',
  styleUrls: ['./esqueci-senha.component.css'],
})
export class EsqueciSenhaComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private securityService: SecurityService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  enviandoCodigo = false;
  codigoEnviado = false;
  carregandoReenvio = false;
  alterandoSenha = false;
  esconderSenha = true;
  esconderConfirmacaoSenha = true;
  countdown: number = 60;

  senhaControl = this.formBuilder.group({
    cpfEmail: new FormControl('', Validators.required),
    codigoSeguranca: new FormControl('', Validators.required),
    novaSenha: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
    confirmaNovaSenha: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
  });

  @ViewChild('cpfEmail') cpfEmailInput: ElementRef;
  @ViewChild('codigo') codigoInput: ElementRef;

  ngOnInit(): void {}

  jaPossuiCodigo() {
    this.codigoEnviado = true;
  }

  enviarCodigo() {
    this.enviandoCodigo = true;

    var cpfEmail = this.senhaControl.get('cpfEmail')?.value;

    if (cpfEmail == '') {
      this.abrirMensagem('Informe seu CPF ou Email');
      return;
    }

    this.securityService.enviarCodigoSegurancaPorCpfEmail(cpfEmail!).subscribe(
      (retorno) => {
        this.dialog.open(MensagemModalComponent, {
          width: '600px',
          data: {
            mensagem:
              'Código de segurança para alterar a senha foi enviado para o email:\n' +
              retorno.email,
            icone: 'check_circle_outline',
          },
        });

        this.enviandoCodigo = false;
        this.codigoEnviado = true;

        this.iniciarContagemRegressiva();
      },
      (error) => {
        if (error.error.mensagem != null)
          this.dialog.open(MensagemModalComponent, {
            width: '600px',
            data: {
              mensagem: error.error.mensagem,
              icone: 'warning',
            },
          });
        else if (error.error.erroInterno != null)
          this.abrirMensagem(error.error.mensagem);
        else
          this.dialog.open(MensagemModalComponent, {
            width: '600px',
            data: {
              mensagem: 'Ocorreu um erro ao enviar o código de segurança',
              icone: 'warning',
            },
          });

        this.enviandoCodigo = false;
      }
    );
  }

  salvarNovaSenha() {
    if (
      this.senhaControl.get('novaSenha')?.value !=
      this.senhaControl.get('confirmaNovaSenha')?.value
    ) {
      this.abrirMensagem('As senhas não são iguais');
      return;
    }

    this.alterandoSenha = true;

    var novaSenha: EsqueciSenha = {
      cpfEmail: this.senhaControl.get('cpfEmail')?.value!,
      codigo: this.senhaControl.get('codigoSeguranca')?.value!,
      novaSenha: this.senhaControl.get('novaSenha')?.value!,
    };

    this.securityService.esqueciSenha(novaSenha).subscribe(
      () => {
        this.dialog.open(MensagemModalComponent, {
          width: '600px',
          data: {
            mensagem: 'Senha atualizada com sucesso',
            icone: 'check_circle_outline',
          },
        });

        this.senhaControl.get('cpfEmail')?.reset();
        this.senhaControl.get('codigoSeguranca')?.reset();
        this.senhaControl.get('novaSenha')?.reset();
        this.senhaControl.get('confirmaNovaSenha')?.reset();

        this.alterandoSenha = false;
      },
      (error) => {
        if (error.error.mensagem != null)
          this.dialog.open(MensagemModalComponent, {
            width: '600px',
            data: {
              mensagem: error.error.mensagem,
              icone: 'warning',
            },
          });
        else if (error.error.erroInterno != null)
          this.abrirMensagem(error.error.mensagem);
        else
          this.dialog
            .open(MensagemModalComponent, {
              width: '600px',
              data: {
                mensagem: 'Ocorreu um erro ao atualizar senha',
                icone: 'warning',
              },
            })
            .afterClosed()
            .subscribe(() => {
              this.dialog.open(LoginComponent);
            });

        this.alterandoSenha = false;
      }
    );
  }

  private iniciarContagemRegressiva() {
    this.countdown = 60;
    this.carregandoReenvio = true;

    const interval = setInterval(() => {
      this.countdown--;

      if (this.countdown === 0) {
        clearInterval(interval);
        this.carregandoReenvio = false;
      }
    }, 1000);
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
