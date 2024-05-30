import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AtletasService } from 'src/app/core/Atletas/atletas.service';

export interface DialogData {
  cpf: string;
  naoPermiteCadastro: boolean;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  handleSuccess($event: string) {
    this.captchaOk = true;
  }

  handleExpire() {
    this.captchaOk = false;
  }

  handleReset() {
    this.captchaOk = false;
  }

  constructor(
    private atletasService: AtletasService,
    private dialogRef: MatDialogRef<LoginComponent>,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  athlete: any;
  cpf: string;
  password: string;
  hide = true;
  cadastroEvento: boolean;
  acessando = false;
  captchaOk = false;

  loginControl = this.formBuilder.group({
    cpf: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
    recaptcha: new FormControl('', Validators.required),
    rememberMe: false,
  });

  ngOnInit(): void {
    if (localStorage.getItem('cpf')) {
      this.cpf = localStorage
        .getItem('cpf')!
        .replace('.', '')
        .replace('.', '')
        .replace('-', '');
    }

    if (this.cpf != null) {
      this.loginControl.get('cpf')?.setValue(this.cpf);
      this.loginControl.get('password')?.setValue('');
      localStorage.removeItem('cpf');
    }

    const savedCredentials = localStorage.getItem('credentials');

    if (savedCredentials) {
      const credentials = JSON.parse(savedCredentials);
      this.loginControl.get('cpf')?.setValue(credentials.cpf);
      this.loginControl.get('password')?.setValue(credentials.password);
      this.loginControl.get('rememberMe')?.setValue(credentials.rememberMe);
    }

    if (this.data.naoPermiteCadastro) this.cadastroEvento = true;
    else this.cadastroEvento = false;
  }

  verifyAthlete() {
    this.acessando = true;

    this.cpf =
      this.loginControl.get('cpf')!.value != undefined
        ? this.loginControl.get('cpf')!.value!.toString()
        : '';
    this.password =
      this.loginControl.get('password')!.value != undefined
        ? this.loginControl.get('password')!.value!.toString()
        : '';

    if (this.cpf == null || this.cpf == '') {
      this.openMessage('Informe seu CPF/CNPJ');
      this.acessando = false;
      return;
    }

    this.realizarLogin();
  }

  private openMessage(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }

  private realizarLogin() {
    this.atletasService.login(this.cpf, this.password).subscribe(
      (token) => {
        var eventId = localStorage.getItem('eventId');
        localStorage.removeItem('eventId');
        localStorage.setItem('currentUser', token);

        if (this.loginControl.get('rememberMe')?.value === true) {
          localStorage.setItem(
            'credentials',
            JSON.stringify(this.loginControl.value)
          );
        } else {
          localStorage.removeItem('credentials');
        }

        this.loginControl.get('cpf')!.setValue('');
        this.loginControl.get('password')!.setValue('');

        if (eventId != null) {
          this.router.navigateByUrl('eventos/' + eventId);
        } else {
          window.location.reload();
        }

        this.acessando = false;

        this.dialogRef.close();
      },
      (error) => {
        if (error.status == 404) {
          this.openMessage('Usuário não encontrado');
        }
        if (error.status == 401) {
          this.openMessage('Senha inválida');
        }

        this.acessando = false;
      },
      () => (this.acessando = false)
    );
  }

  novoAtleta() {
    var eventId = localStorage.getItem('eventId');
    localStorage.removeItem('eventId');

    this.router.navigateByUrl('eventos/' + eventId);

    this.dialogRef.close();
  }

  ok(ok?: any) {}
}
