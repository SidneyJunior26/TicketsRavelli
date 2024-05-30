import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { NovaSenhaCodigo } from 'src/app/shared/models/Atleta/nova-senha';

@Component({
  selector: 'app-senha-primeiro-acesso',
  templateUrl: './senha-primeiro-acesso.component.html',
  styleUrls: ['./senha-primeiro-acesso.component.css'],
})
export class SenhaPrimeiroAcessoComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private securityService: SecurityService,
    private snackBar: MatSnackBar,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  senhaControl = this.formBuilder.group({
    codigoSeguranca: new FormControl('', Validators.required),
    novaSenha: new FormControl('', Validators.required),
    confirmarSenha: new FormControl('', Validators.required),
  });

  atleta: Atleta;
  codigoSeguranca: string;
  idAtleta: string;

  loading: boolean = false;
  countdown: number = 120; // 2 minutos em segundos

  ngOnInit(): void {
    this.idAtleta = this.activatedRoute.snapshot.params['id'];

    this.enviarCodigo();
  }

  protected enviarCodigo() {
    this.securityService.enviarCodigoSegurancaPorId(this.idAtleta).subscribe(
      (codigo) => {
        this.codigoSeguranca = codigo;

        this.iniciarContagemRegressiva();

        this.router.navigateByUrl('/');

        this.abrirMensagem('Código de segurança enviado para o email');
      },
      (error) => {
        this.abrirMensagem(
          'Ocorreu um erro ao enviar o código de segurança para seu email. Contate nossos administradores'
        );
      }
    );
  }

  iniciarContagemRegressiva() {
    this.countdown = 120;
    this.loading = true;

    const interval = setInterval(() => {
      this.countdown--;

      if (this.countdown === 0) {
        clearInterval(interval);
        this.loading = false;
      }
    }, 1000);
  }

  salvarNovaSenha() {
    if (
      this.senhaControl.get('novaSenha')?.value !=
      this.senhaControl.get('confirmarSenha')?.value
    ) {
      this.abrirMensagem('As senhas não são iguais');
      return;
    }

    var novaSenha: NovaSenhaCodigo = {
      id: this.idAtleta,
      codigo: this.senhaControl.get('codigoSeguranca')?.value!,
      novaSenha: this.senhaControl.get('novaSenha')?.value!,
    };

    this.securityService.alterarPrimeiraSenha(novaSenha).subscribe(
      () => {
        this.abrirMensagem('Senha atualizada com sucesso');

        this.senhaControl.get('codigoSeguranca')?.reset();
        this.senhaControl.get('novaSenha')?.reset();
        this.senhaControl.get('confirmarSenha')?.reset();
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
