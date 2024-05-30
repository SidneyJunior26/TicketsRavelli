import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Subject, Subscription, interval, switchMap, takeWhile } from 'rxjs';
import { BoletoService } from 'src/app/core/Boletos/boleto.service';
import { PixService } from 'src/app/core/Pix/pix.service';
import { MensagemModalComponent } from 'src/app/shared/mensagem-modal/mensagem-modal.component';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { BoletoInputModel } from 'src/app/shared/models/boletoInputModel';
import { Evento } from 'src/app/shared/models/evento';
import { PixInputModel } from 'src/app/shared/models/pixInputModel';

export interface DialogData {
  idInscricao: number;
  atleta: Atleta;
  evento: Evento;
  valorPagamento: number;
  cupomDesconto: string;
}

@Component({
  selector: 'app-formas-pagamentos',
  templateUrl: './formas-pagamentos.component.html',
  styleUrls: ['./formas-pagamentos.component.css'],
})
export class FormasPagamentosComponent implements OnInit, OnDestroy {
  constructor(
    private pixService: PixService,
    private boletoService: BoletoService,
    public dialogRef: MatDialogRef<FormasPagamentosComponent>,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  private intervaloSub: Subscription;

  pagamentoConfirmado = false;
  gerandoBoleto = false;
  erroBoleto = false;

  gerandoPix = false;
  erroPix = false;
  cpfInvalido = false;
  confirmandoPagamentoPix = false;

  step = -1;

  linkBoleto = '';

  qrCodeImageSrc = '';
  qrCode = '';

  ngOnInit(): void {}

  ngOnDestroy() {
    // Cancela a execução do intervalo ao destruir o componente
    if (this.intervaloSub) {
      this.intervaloSub.unsubscribe();
    }
  }

  setStep(index: number) {
    this.step = index;

    if (index == 0 && this.qrCode == '') this.gerarPix();
    else if (index == 1 && this.linkBoleto == '') this.gerarBoleto();
  }

  gerarBoleto() {
    this.linkBoleto = '';

    this.gerandoBoleto = true;
    this.erroBoleto = false;

    var boletoModel = new BoletoInputModel(
      this.data.atleta!.nome,
      this.data.atleta.email,
      this.data.atleta.cpf,
      this.data.atleta.celular,
      this.data.evento.nome,
      this.data.valorPagamento.toFixed(2),
      1,
      this.data.cupomDesconto
    );

    this.boletoService
      .gerarBoleto(this.data.idInscricao, boletoModel)
      .subscribe(
        (retorno) => {
          this.linkBoleto = retorno.link;

          this.gerandoBoleto = false;
        },
        (retorno) => {
          if (retorno.error.errorMessage != '') {
            let mensagem = '';

            if (retorno.error.errorMessage.contains('CPF')) {
              mensagem = 'Documento CPF é inválido';
              this.cpfInvalido = true;
            }

            this.dialog.open(MensagemModalComponent, {
              width: '600px',
              data: {
                mensagem:
                  'Ocorreu um erro ao tentar gerar o Boleto: ' + mensagem != ''
                    ? ': ' + mensagem
                    : '',
                icone: 'warning',
              },
            });
          }

          this.gerandoBoleto = false;
          this.erroBoleto = true;
        }
      );
  }

  gerarPix() {
    this.gerandoPix = true;
    this.erroPix = false;
    this.cpfInvalido = false;

    var pixModel = new PixInputModel(
      this.data.idInscricao,
      this.data.cupomDesconto
    );

    this.pixService.gerarPix(pixModel).subscribe(
      (retorno) => {
        this.qrCodeImageSrc = retorno.qrCodeImg;
        this.qrCode = retorno.qrCode;

        this.gerandoPix = false;

        this.aguardaPagamentoPix();
      },
      (retorno) => {
        if (retorno.error.errorMessage != '') {
          let mensagem = '';

          if (retorno.error.errorMessage.contains('CPF')) {
            mensagem = 'Documento CPF é inválido';
            this.cpfInvalido = true;
          }

          this.dialog.open(MensagemModalComponent, {
            width: '600px',
            data: {
              mensagem:
                'Ocorreu um erro ao tentar gerar o Pix: ' + mensagem != ''
                  ? ': ' + mensagem
                  : '',
              icone: 'warning',
            },
          });
        }

        this.gerandoPix = false;

        this.erroPix = true;
      },
      () => {
        this.gerandoPix = false;
      }
    );
  }

  private aguardaPagamentoPix() {
    const stopInterval$ = new Subject<void>();

    this.intervaloSub = interval(10000)
      .pipe(
        switchMap(async () => {
          const retorno = await this.pixService
            .consultarPix(this.data.idInscricao)
            .toPromise();

          if (retorno.pago) {
            this.pagamentoPixConfirmado();
            this.fecharPix();
            stopInterval$.next(); // Para o intervalo
          }
        }),
        takeWhile(() => !stopInterval$.closed)
      )
      .subscribe();
  }

  consultarStatusPix(inscricaoId: number) {
    this.confirmandoPagamentoPix = true;

    this.pixService.consultarPix(inscricaoId).subscribe((retorno) => {
      if (retorno.pago == true) {
        this.confirmandoPagamentoPix = false;

        this.pagamentoPixConfirmado();

        this.fecharPix();
      }
    });
  }

  private pagamentoPixConfirmado() {
    this.dialog
      .open(MensagemModalComponent, {
        width: '600px',
        data: {
          mensagem: 'Pagamento confirmado com sucesso.',
          icone: 'check_circle',
        },
      })
      .afterClosed()
      .subscribe(() => {
        this.fecharPix();
      });
  }

  private fecharPix() {
    setInterval(() => {
      this.router.navigateByUrl('cadastro/inscricoes');
      this.fechar();
    }, 3000);
  }

  abrirMensagemPixCopiado() {
    this.snackBar.open('Código pix copiado', 'OK', {
      duration: 2000,
    });
  }

  visualizarBoleto() {
    window.open(this.linkBoleto, '_blank');
    this.router.navigateByUrl('cadastro/inscricoes');
    this.fechar();
  }

  fechar() {
    this.dialogRef.close();
  }

  cancelar() {
    this.router.navigateByUrl('cadastro/inscricoes');
    this.fechar();
  }
}
