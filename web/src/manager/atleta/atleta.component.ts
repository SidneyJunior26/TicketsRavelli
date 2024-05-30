import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AtletasService } from 'src/app/core/Atletas/atletas.service';
import { DadosMedicosService } from 'src/app/core/RegistrosMedicos/dados-medicos.service';
import { ExternalService } from 'src/app/core/Shared/external.service';
import { MensagemModalComponent } from 'src/app/shared/mensagem-modal/mensagem-modal.component';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { AtualizaAtleta } from 'src/app/shared/models/Atleta/atualizar-atleta';
import { DadosMedicos } from 'src/app/shared/models/dadosMedicos';

export interface DialogData {
  atleta: Atleta;
}

@Component({
  selector: 'app-atleta',
  templateUrl: './atleta.component.html',
  styleUrls: ['./atleta.component.css'],
})
export class AtletaComponent implements OnInit {
  constructor(
    private externalService: ExternalService,
    private atletaService: AtletasService,
    private dadosMedicosService: DadosMedicosService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  novoAtleta: boolean = true;
  atleta: Atleta;

  cadastrandoAtleta: boolean = false;

  atletaControl = this.formBuilder.group({
    id: new FormControl(''),
    nome: new FormControl('', Validators.required),
    nascimento: new FormControl('', Validators.required),
    sexo: new FormControl('1', Validators.required),
    cpf: new FormControl('', Validators.required),
    rg: new FormControl('', Validators.required),
    responsavel: new FormControl(''),
    endereco: new FormControl(''),
    numero: new FormControl(''),
    complemento: new FormControl(''),
    cep: new FormControl(''),
    cidade: new FormControl(''),
    uf: new FormControl(''),
    pais: new FormControl(''),
    telefone: new FormControl(''),
    celular: new FormControl('', Validators.required),
    email: new FormControl('', Validators.email),
    profissao: new FormControl(''),
    emergenciaContato: new FormControl('', Validators.required),
    emergenciaTelefone: new FormControl(''),
    emergenciaCelular: new FormControl('', Validators.required),
    camisa: new FormControl('Masculino P'),
    camisaCiclismo: new FormControl('Masculino P'),
    mktLojaPreferida: new FormControl(''),
    mktBikePreferida: new FormControl(''),
    mktAro: new FormControl(''),
    mktCambio: new FormControl(''),
    mktFreio: new FormControl(''),
    mktSuspensao: new FormControl(''),
    mktMarcaPneu: new FormControl(''),
    mktModeloPneu: new FormControl(''),
    mktTenis: new FormControl(''),
    federacao: new FormControl(''),
    senha: new FormControl(''),
  });

  ngOnInit(): void {
    if (this.data.atleta != null) {
      this.atleta = this.data.atleta;
      this.carregarAtletaControl();
      this.novoAtleta = false;
    } else {
      this.novoAtleta = true;
    }
  }

  salvarAtleta() {
    this.cadastrandoAtleta = true;

    if (this.novoAtleta) {
      this.cadastrarAtleta();
    } else {
      this.atualizarAtleta();
    }
  }

  carregarEndereco(cep: string) {
    cep = cep.replace('-', '');

    if (cep.length < 8) return;

    this.externalService.consultarEndereçoPorCEP(cep).subscribe((adress) => {
      this.atletaControl.get('endereco')?.setValue(adress.logradouro);
      this.atletaControl.get('cidade')?.setValue(adress.localidade);
      this.atletaControl.get('uf')?.setValue(adress.uf);
    });
  }

  private atualizarAtleta() {
    var atletaAtualizado: AtualizaAtleta = {
      nome: this.atletaControl.get('nome')?.value!,
      nascimento: this.atletaControl.get('nascimento')?.value!,
      sexo: this.atletaControl.get('sexo')?.value!,
      cpf: this.atletaControl.get('cpf')?.value!,
      rg: this.atletaControl.get('rg')?.value!,
      responsavel: this.atletaControl.get('responsavel')?.value!,
      endereco: this.atletaControl.get('endereco')?.value!,
      numero: this.atletaControl.get('numero')?.value!,
      complemento: this.atletaControl.get('complemento')?.value!,
      cep: this.atletaControl.get('cep')?.value!,
      cidade: this.atletaControl.get('cidade')?.value!,
      uf: this.atletaControl.get('uf')?.value!,
      pais: this.atletaControl.get('pais')?.value!,
      telefone: this.atletaControl.get('telefone')?.value!,
      celular: this.atletaControl.get('celular')?.value!,
      email: this.atletaControl.get('email')?.value!,
      profissao: this.atletaControl.get('profissao')?.value!,
      emergenciaContato: this.atletaControl.get('emergenciaContato')?.value!,
      emergenciaFone: this.atletaControl.get('emergenciaTelefone')?.value!,
      emergenciaCelular: this.atletaControl.get('emergenciaCelular')?.value!,
      camisa: this.atletaControl.get('camisa')?.value!,
      camisaCiclismo: this.atletaControl.get('camisaCiclismo')?.value!,
      mktLojaPreferida: this.atletaControl.get('mktLojaPreferida')?.value!,
      mktBikePreferida: this.atletaControl.get('mktBikePreferida')?.value!,
      mktAro: this.atletaControl.get('mktAro')?.value!,
      mktCambio: this.atletaControl.get('mktCambio')?.value!,
      mktFreio: this.atletaControl.get('mktFreio')?.value!,
      mktSuspensao: this.atletaControl.get('mktSuspensao')?.value!,
      mktMarcaPneu: this.atletaControl.get('mktMarcaPneu')?.value!,
      mktModeloPneu: this.atletaControl.get('mktModeloPneu')?.value!,
      mktTenis: this.atletaControl.get('mktTenis')?.value!,
      federacao: this.atletaControl.get('federacao')?.value!,
    };

    this.atletaService
      .atualizarAtleta(atletaAtualizado, this.atleta.id!)
      .subscribe(
        () => {
          this.abrirMensagem('Dados atualizados com sucesso.');

          this.cadastrandoAtleta = false;
        },
        (error) => {
          this.abrirMensagem('Ocorreu um erro ao atualizar os dados');

          this.cadastrandoAtleta = false;
        }
      );
  }

  private cadastrarAtleta() {
    if (
      this.atletaControl.get('senha')!.value == null ||
      this.atletaControl.get('senha')!.value?.toString() == ''
    ) {
      this.abrirMensagem('Informe a Senha!');
      return;
    }

    const novoAtleta: Atleta = {
      nome: this.atletaControl.get('nome')!.value!.toString(),
      nascimento: this.atletaControl.get('nascimento')!.value!,
      sexo: this.atletaControl.get('sexo')!.value!.toString(),
      cpf: this.atletaControl.get('cpf')!.value!.toString(),
      rg: this.atletaControl.get('rg')!.value!.toString(),
      responsavel:
        this.atletaControl.get('responsavel')!.value != null
          ? this.atletaControl.get('responsavel')!.value!.toString()
          : '',
      endereco:
        this.atletaControl.get('endereco')!.value != null
          ? this.atletaControl.get('endereco')!.value!.toString()
          : '',
      numero:
        this.atletaControl.get('numero')!.value != null
          ? this.atletaControl.get('numero')!.value!.toString()
          : '',
      complemento:
        this.atletaControl.get('complemento')!.value != null
          ? this.atletaControl.get('complemento')!.value!.toString()
          : '',
      cep:
        this.atletaControl.get('cep')!.value != null
          ? this.atletaControl.get('cep')!.value!.toString()
          : '',
      cidade:
        this.atletaControl.get('cidade')!.value != null
          ? this.atletaControl.get('cidade')!.value!.toString()
          : '',
      uf:
        this.atletaControl.get('uf')!.value != null
          ? this.atletaControl.get('uf')!.value!.toString()
          : '',
      pais:
        this.atletaControl.get('pais')!.value != null
          ? this.atletaControl.get('pais')!.value!.toString()
          : '',
      telefone:
        this.atletaControl.get('telefone')!.value != null
          ? this.atletaControl.get('telefone')!.value!.toString()
          : '',
      celular:
        this.atletaControl.get('celular')!.value != null
          ? this.atletaControl.get('celular')!.value!.toString()
          : '',
      email:
        this.atletaControl.get('email')!.value != null
          ? this.atletaControl.get('email')!.value!.toString()
          : '',
      profissao:
        this.atletaControl.get('profissao')!.value != null
          ? this.atletaControl.get('profissao')!.value!.toString()
          : '',
      emergenciaContato:
        this.atletaControl.get('emergenciaContato')!.value != null
          ? this.atletaControl.get('emergenciaContato')!.value!.toString()
          : '',
      emergenciaFone:
        this.atletaControl.get('emergenciaTelefone')!.value != null
          ? this.atletaControl.get('emergenciaTelefone')!.value!.toString()
          : '',
      emergenciaCelular:
        this.atletaControl.get('emergenciaCelular')!.value != null
          ? this.atletaControl.get('emergenciaCelular')!.value!.toString()
          : '',
      camisa: '',
      camisaCiclismo: '',
      mktLojaPreferida:
        this.atletaControl.get('mktLojaPreferida')!.value != null
          ? this.atletaControl.get('mktLojaPreferida')!.value!.toString()
          : '',
      mktBikePreferida:
        this.atletaControl.get('mktBikePreferida')!.value != null
          ? this.atletaControl.get('mktBikePreferida')!.value!.toString()
          : '',
      mktAro:
        this.atletaControl.get('mktAro')!.value != null
          ? this.atletaControl.get('mktAro')!.value!.toString()
          : '',
      mktCambio:
        this.atletaControl.get('mktCambio')!.value != null
          ? this.atletaControl.get('mktCambio')!.value!.toString()
          : '',
      mktFreio:
        this.atletaControl.get('mktFreio')!.value != null
          ? this.atletaControl.get('mktFreio')!.value!.toString()
          : '',
      mktSuspensao:
        this.atletaControl.get('mktSuspensao')!.value != null
          ? this.atletaControl.get('mktSuspensao')!.value!.toString()
          : '',
      mktMarcaPneu:
        this.atletaControl.get('mktMarcaPneu')!.value != null
          ? this.atletaControl.get('mktMarcaPneu')!.value!.toString()
          : '',
      mktModeloPneu:
        this.atletaControl.get('mktModeloPneu')!.value != null
          ? this.atletaControl.get('mktModeloPneu')!.value!.toString()
          : '',
      mktTenis:
        this.atletaControl.get('mktTenis')!.value != null
          ? this.atletaControl.get('mktTenis')!.value!.toString()
          : '',
      federacao:
        this.atletaControl.get('federacao')!.value != null
          ? this.atletaControl.get('federacao')!.value!.toString()
          : '',
      acesso: this.atletaControl.get('senha')!.value!.toString(),
    };

    this.atleta = novoAtleta;

    this.atletaService.cadastrarAtletaAdm(novoAtleta).subscribe(
      (retorno) => {
        this.cadastrarDadosMedicos(retorno.idAtleta);

        this.abrirMensagem('Atleta Cadastrado com Sucesso.');

        this.cadastrandoAtleta = false;
      },
      (error) => {
        if (error.errors[1]) {
          this.abrirMensagem(error.errors[1]);
        } else if (error.message != null) {
          this.abrirMensagem(error.message);
        }

        this.cadastrandoAtleta = false;
      }
    );
  }

  private cadastrarDadosMedicos(idAtleta: string) {
    var dadosMedicos: DadosMedicos = {
      idAtleta: idAtleta,
      plano: 0,
      planoEmpresa: '',
      planoTipo: '',
      pressaoAlta: 0,
      desmaio: 0,
      cardiaco: 0,
      diabetes: 0,
      asma: 0,
      alergia: 0,
      alergiaQual: '',
      cirurgia: 0,
      cirurgiaQual: '',
      medicacao: 0,
      medicacaoQual: '',
      medicacaoTempo: '',
      malestar: 0,
      malestarQual: '',
      acompanhamento: 0,
      acompanhamentoQual: '',
      outros: '',
    };

    this.dadosMedicosService.cadastrarDadosMedicos(dadosMedicos).subscribe();
  }

  private carregarAtletaControl() {
    for (const key in this.atleta) {
      if (this.atletaControl.get(key)) {
        this.atletaControl.get(key)?.setValue(this.atleta[key]);
      }
    }
  }

  verificarUsuarioJaExisteCPF() {
    var cpf = this.atletaControl.get('cpf')!.value!.toString();

    if (this.atletaControl.get('cpf')!.value == null) return;

    if (this.atleta != null) {
      if (this.atleta.cpf == cpf) return;
    }

    this.atletaService.verificarUsuarioExiste(cpf).subscribe(() => {
      this.dialog.open(MensagemModalComponent, {
        width: '600px',
        data: {
          mensagem: 'CPF já cadastrado no sistema.',
          icone: 'warning',
        },
      });

      this.atletaControl.get('cpf')!.setValue('');
    });
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
