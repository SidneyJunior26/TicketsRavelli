import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EventosService } from 'src/app/core/Eventos/events.service';
import { RegulamentoService } from 'src/app/core/Regulamento/regulatmento.service';
import { ImagemService } from 'src/app/core/Shared/imagem.service';
import { ImageUploadComponent } from 'src/app/shared/image-upload/image-upload.component';
import { Evento } from 'src/app/shared/models/evento';
import { Regulamento } from 'src/app/shared/models/regulamento';

export interface DialogData {
  eventoId: number;
}

@Component({
  selector: 'app-evento',
  templateUrl: './evento.component.html',
  styleUrls: ['./evento.component.css'],
})
export class EventoComponent implements OnInit {
  @ViewChild('imageUpload') imageUpload: ImageUploadComponent;

  constructor(
    private eventoService: EventosService,
    private regulamentoService: RegulamentoService,
    private imagemService: ImagemService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  novoEvento: boolean = true;
  evento: Evento;

  eventoControl = this.formBuilder.group({
    id: new FormControl(''),
    nome: new FormControl('', Validators.required),
    descricao: new FormControl(''),
    local: new FormControl(''),
    data: new FormControl('', Validators.required),
    dataIniInscricao: new FormControl('', Validators.required),
    dataFimInscricao: new FormControl('', Validators.required),
    dataDesconto: new FormControl('', Validators.required),
    dataValorNormal: new FormControl(''),
    valor1: new FormControl(0, [Validators.required, Validators.min(1)]),
    valor2: new FormControl(0, [Validators.required, Validators.min(1)]),
    valorNormal: new FormControl(0),
    pacote2V1: new FormControl(0),
    pacote2V2: new FormControl(0),
    pacote2V3: new FormControl(0),
    pacote3V1: new FormControl(0),
    pacote3V2: new FormControl(0),
    pacote3V3: new FormControl(0),
    pacote4V1: new FormControl(0),
    pacote4V2: new FormControl(0),
    pacote4V3: new FormControl(0),
    pacote1Desc: new FormControl(''),
    pacote2Desc: new FormControl(''),
    pacote3Desc: new FormControl(''),
    pacote4Desc: new FormControl(''),
    pacote1Ativo: new FormControl(0, Validators.required),
    pacote2Ativo: new FormControl(0, Validators.required),
    pacote3Ativo: new FormControl(0, Validators.required),
    pacote4Ativo: new FormControl(0, Validators.required),
    categoria: new FormControl('', Validators.required),
    boletoInf1: new FormControl(''),
    boletoInf2: new FormControl(''),
    boletoInf3: new FormControl(''),
    boletoInstrucao1: new FormControl(''),
    boletoInstrucao2: new FormControl(''),
    boletoInstrucao3: new FormControl(''),
    obsTela: new FormControl(''),
    txtEmailCadastro: new FormControl(''),
    txtEmailBaixa: new FormControl(''),
    ativaInscricao: new FormControl(1, Validators.required),
    ativaEvento: new FormControl(1, Validators.required),
    ativaAlteracaoInscricao: new FormControl(1, Validators.required),
    eventoTipo: new FormControl('0', Validators.required),
    pacote1V1Pseg: new FormControl(''),
    pacote1V2Pseg: new FormControl(''),
    pacote1V3Pseg: new FormControl(''),
    pacote2V1Pseg: new FormControl(''),
    pacote2V2Pseg: new FormControl(''),
    pacote2V3Pseg: new FormControl(''),
    pacote3V1Pseg: new FormControl(''),
    pacote3V2Pseg: new FormControl(''),
    pacote3V3Pseg: new FormControl(''),
    pacote4V1Pseg: new FormControl(''),
    pacote4V2Pseg: new FormControl(''),
    pacote4V3Pseg: new FormControl(''),
  });

  ngOnInit(): void {
    if (
      this.data != null &&
      this.data.eventoId != null &&
      this.data.eventoId != undefined
    ) {
      this.eventoService
        .consultarEventoPeloId(this.data.eventoId)
        .subscribe((evento) => {
          this.evento = evento;
          this.novoEvento = false;
          this.carregarEventoControl();
        });
    } else {
      this.novoEvento = true;
    }
  }

  salvarEvento() {
    var evento: Evento = {
      id: 0,
      nome: this.eventoControl.get('nome')?.value!,
      descricao: this.eventoControl.get('descricao')!.value!,
      local: this.eventoControl.get('local')!.value!,
      data: this.eventoControl.get('data')!.value!,
      dataIniInscricao: this.eventoControl.get('dataIniInscricao')!.value!,
      dataFimInscricao: this.eventoControl.get('dataFimInscricao')!.value!,
      dataDesconto: this.eventoControl.get('dataDesconto')!.value!,
      dataValorNormal: this.eventoControl.get('dataValorNormal')!.value!,
      valor1: this.eventoControl.get('valor1')!.value!,
      valor2: this.eventoControl.get('valor2')!.value!,
      valorNormal: this.eventoControl.get('valorNormal')!.value!,
      pacote2V1: this.eventoControl.get('pacote2V1')!.value!,
      pacote2V2: this.eventoControl.get('pacote2V2')!.value!,
      pacote2V3: this.eventoControl.get('pacote2V3')!.value!,
      pacote3V1: this.eventoControl.get('pacote3V1')!.value!,
      pacote3V2: this.eventoControl.get('pacote3V2')!.value!,
      pacote3V3: this.eventoControl.get('pacote3V3')!.value!,
      pacote4V1: this.eventoControl.get('pacote4V1')!.value!,
      pacote4V2: this.eventoControl.get('pacote4V2')!.value!,
      pacote4V3: this.eventoControl.get('pacote4V3')!.value!,
      pacote1Desc: this.eventoControl.get('pacote1Desc')!.value!,
      pacote2Desc: this.eventoControl.get('pacote2Desc')!.value!,
      pacote3Desc: this.eventoControl.get('pacote3Desc')!.value!,
      pacote4Desc: this.eventoControl.get('pacote4Desc')!.value!,
      pacote1Ativo: Number(this.eventoControl.get('pacote1Ativo')!.value!),
      pacote2Ativo: Number(this.eventoControl.get('pacote2Ativo')!.value!),
      pacote3Ativo: Number(this.eventoControl.get('pacote3Ativo')!.value!),
      pacote4Ativo: Number(this.eventoControl.get('pacote4Ativo')!.value!),
      categoria: this.eventoControl.get('categoria')!.value!,
      boletoInf1: this.eventoControl.get('boletoInf1')!.value!,
      boletoInf2: this.eventoControl.get('boletoInf2')!.value!,
      boletoInf3: this.eventoControl.get('boletoInf3')!.value!,
      boletoInstrucao1: this.eventoControl.get('boletoInstrucao1')!.value!,
      boletoInstrucao2: this.eventoControl.get('boletoInstrucao2')!.value!,
      boletoInstrucao3: this.eventoControl.get('boletoInstrucao3')!.value!,
      obsTela: this.eventoControl.get('obsTela')?.value!,
      txtEmailCadastro: this.eventoControl.get('txtEmailCadastro')?.value!,
      txtEmailBaixa: this.eventoControl.get('txtEmailBaixa')?.value!,
      ativaInscricao: Number(this.eventoControl.get('ativaInscricao')?.value!),
      ativaEvento: Number(this.eventoControl.get('ativaEvento')?.value!),
      ativaAlteracaoInscricao: Number(
        this.eventoControl.get('ativaAlteracaoInscricao')?.value!
      ),
      eventoTipo: Number(this.eventoControl.get('eventoTipo')?.value!),
      pacote1V1Pseg: this.eventoControl.get('pacote1V1Pseg')?.value!,
      pacote1V2Pseg: this.eventoControl.get('pacote1V2Pseg')?.value!,
      pacote1V3Pseg: this.eventoControl.get('pacote1V3Pseg')?.value!,
      pacote2V1Pseg: this.eventoControl.get('pacote2V1Pseg')?.value!,
      pacote2V2Pseg: this.eventoControl.get('pacote2V2Pseg')?.value!,
      pacote2V3Pseg: this.eventoControl.get('pacote2V3Pseg')?.value!,
      pacote3V1Pseg: this.eventoControl.get('pacote3V1Pseg')?.value!,
      pacote3V2Pseg: this.eventoControl.get('pacote3V2Pseg')?.value!,
      pacote3V3Pseg: this.eventoControl.get('pacote3V3Pseg')?.value!,
      pacote4V1Pseg: this.eventoControl.get('pacote4V1Pseg')?.value!,
      pacote4V2Pseg: this.eventoControl.get('pacote4V2Pseg')?.value!,
      pacote4V3Pseg: this.eventoControl.get('pacote4V3Pseg')?.value!,
    };

    if (this.eventoControl.get('id')?.value) {
      evento.id = Number(this.eventoControl.get('id')!.value!);
    }

    if (evento.id == 0) {
      this.eventoService.cadastrarEvento(evento).subscribe((idEvento) => {
        this.evento.id = idEvento;
        this.abrirMensagem('Evento Cadastrado com Sucesso!');
        this.criarRegulamento(idEvento);
        this.salvarImagemEvento();
      });
    } else {
      this.eventoService.atualizarEvento(evento).subscribe((evento) => {
        this.abrirMensagem('Evento Atualizado com Sucesso!');
        this.salvarImagemEvento();
      });
    }
  }

  private carregarEventoControl() {
    for (const key in this.evento) {
      if (this.eventoControl.get(key)) {
        this.eventoControl.get(key)?.setValue(this.evento[key]);
      }
    }
  }

  private carregarImagemEvento() {}

  private salvarImagemEvento() {
    const imagem = this.imageUpload.getImagem();

    if (imagem == null || imagem == undefined) {
      return;
    }

    this.imagemService.salvarImagem(this.evento.id!, imagem).subscribe();
  }

  private criarRegulamento(idEvento: number) {
    const regulamento: Regulamento = {
      idEvento: idEvento,
      regulamento1: '',
      compromisso: '',
    };

    this.regulamentoService.cadastrarRegulamento(regulamento).subscribe();
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
