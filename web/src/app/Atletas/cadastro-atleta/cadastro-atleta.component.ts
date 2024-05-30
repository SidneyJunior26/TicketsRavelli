import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { AtletasService } from 'src/app/core/Atletas/atletas.service';
import { SubscriptionsService } from 'src/app/core/Inscricoes/inscricoes.service';
import { DadosMedicosService } from 'src/app/core/RegistrosMedicos/dados-medicos.service';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { ExternalService } from 'src/app/core/Shared/external.service';
import { LoginComponent } from 'src/app/shared/login/login.component';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { DadosMedicos } from 'src/app/shared/models/dadosMedicos';
import { NavbarComponent } from 'src/app/shared/navbar/navbar.component';
import { AtualizacaoInscricaoComponent } from '../atualizacao-inscricao/atualizacao-inscricao.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NovaSenha } from 'src/app/shared/models/Atleta/nova-senha';
import { AtualizaAtleta } from 'src/app/shared/models/Atleta/atualizar-atleta';
import { Router } from '@angular/router';
import { MatTabGroup } from '@angular/material/tabs';
import { InscricoesAtleta } from 'src/app/shared/models/inscricoesAtleta';
import { EventosService } from 'src/app/core/Eventos/events.service';
import { FormasPagamentosComponent } from 'src/app/Pagamentos/formas-pagamentos/formas-pagamentos.component';
import { Subscription } from 'src/app/shared/models/inscricao';
import { Evento } from 'src/app/shared/models/evento';

@Component({
  selector: 'app-cadastro-atleta',
  templateUrl: './cadastro-atleta.component.html',
  styleUrls: ['./cadastro-atleta.component.css'],
})
export class CadastroAtletaComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private externalServices: ExternalService,
    private securityService: SecurityService,
    private atletaService: AtletasService,
    private inscricoesService: SubscriptionsService,
    private eventosService: EventosService,
    private navBar: NavbarComponent,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private medicalService: DadosMedicosService,
    private router: Router
  ) {}

  @ViewChild('paginatorInscricoes') paginatorInscricoes: MatPaginator;
  @ViewChild('inputNumero') inputNumero: ElementRef;
  @ViewChild('tabGroup') tabGroup: MatTabGroup;

  athleteControl = this.formBuilder.group({
    nome: new FormControl('', Validators.required),
    nascimento: new FormControl('', Validators.required),
    sexo: new FormControl('1', Validators.required),
    cpf: new FormControl('', Validators.required),
    rg: new FormControl('', Validators.required),
    responsavel: new FormControl(''),
    endereco: new FormControl('', Validators.required),
    numero: new FormControl('', Validators.required),
    complemento: new FormControl(''),
    cep: new FormControl('', Validators.required),
    cidade: new FormControl('', Validators.required),
    uf: new FormControl('', Validators.required),
    pais: new FormControl('', Validators.required),
    telefone: new FormControl(''),
    celular: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    profissao: new FormControl(''),
    emergenciaContato: new FormControl('', Validators.required),
    emergenciaFone: new FormControl(''),
    emergenciaCelular: new FormControl('', Validators.required),
    camisa: new FormControl('Masculino P'),
    camisaCiclismo: new FormControl('Masculino P'),
    mktLojaPreferida: new FormControl(''),
    mktBikePreferida: new FormControl(''),
    mktAro: new FormControl('Aro 29'),
    mktCambio: new FormControl(''),
    mktFreio: new FormControl(''),
    mktSuspensao: new FormControl(''),
    mktMarcaPneu: new FormControl(''),
    mktModeloPneu: new FormControl(''),
    mktTenis: new FormControl(''),
    federacao: new FormControl(''),
  });

  medicalControl = this.formBuilder.group({
    plano: new FormControl(false, Validators.required),
    planoEmpresa: new FormControl(''),
    planoTipo: new FormControl(''),
    pressaoalta: new FormControl(false, Validators.required),
    desmaio: new FormControl(false, Validators.required),
    cardiaco: new FormControl(false, Validators.required),
    diabetes: new FormControl(false, Validators.required),
    asma: new FormControl(false, Validators.required),
    alergia: new FormControl(false, Validators.required),
    alergiaQual: new FormControl(''),
    cirurgia: new FormControl(false, Validators.required),
    cirurgiaQual: new FormControl(''),
    medicacao: new FormControl(false, Validators.required),
    medicacaoQual: new FormControl(''),
    medicacaoTempo: new FormControl(''),
    malestar: new FormControl(false, Validators.required),
    malestarQual: new FormControl(''),
    acompanhamento: new FormControl(false, Validators.required),
    acompanhamentoQual: new FormControl(''),
    outros: new FormControl(''),
  });

  senhaControl = this.formBuilder.group({
    senhaAtual: new FormControl('', Validators.required),
    novaSenha: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
    confirmarSenha: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
  });

  cpfAtleta: string;
  idAtleta: string;
  esconderSenha = true;
  esconderNovaSenha = true;
  esconderConfirmacaoSenha = true;
  semInscricoes = false;
  gerandoBoleto = false;

  atleta: Atleta;
  dadosMedicos: DadosMedicos;
  inscricoesAtleta: InscricoesAtleta[] = [];

  validandoSenha: boolean = false;

  dsInscricoes: MatTableDataSource<InscricoesAtleta>;
  displayedColumns = [
    'editarInscricao',
    'evento',
    'categoria',
    'pago',
    'opcoes',
  ];

  ngOnInit(): void {
    var token = this.securityService.getToken();

    if (token != '') {
      var userInfo = this.securityService.getDecodedAccessToken(token);
    }

    if (userInfo != null) {
      this.navBar.checkLogIn();
      this.cpfAtleta = userInfo.cpf;
      this.idAtleta = userInfo.ID;
      this.consultarAtleta();
      this.carregarInscricoes();
    } else {
      this.router.navigateByUrl('/');
      this.dialog.open(LoginComponent);
    }
  }

  ngAfterViewInit() {
    if (this.abreInscricoes()) {
      this.abrirTab(3);
    }
  }

  private abreInscricoes(): boolean {
    const url = this.router.url;
    return url.includes('inscricoes');
  }

  private abrirTab(index: number) {
    this.tabGroup.selectedIndex = index;
  }

  private consultarAtleta() {
    this.atletaService.consultarAtletaPorCPF(this.cpfAtleta).subscribe(
      (atleta) => {
        this.atleta = atleta;
        this.dadosMedicos = atleta.registroMedico;

        if (this.atleta != null) {
          this.carregarAtleta();
        }
      },
      (error) => {
        if (error.status == 401) {
          this.securityService.logOutToken();
          localStorage.setItem('cpf', this.cpfAtleta);
          this.dialog.open(LoginComponent);
        }
      }
    );
  }

  private carregarAtleta() {
    for (const key in this.atleta) {
      if (this.athleteControl.get(key)) {
        this.athleteControl.get(key)?.setValue(this.atleta[key]);
      }
    }

    if (this.dadosMedicos != null) {
      this.carregarDadosMedicos();
    }
  }

  private carregarDadosMedicos() {
    for (const key in this.dadosMedicos) {
      if (this.medicalControl.get(key)) {
        this.medicalControl.get(key)?.setValue(this.dadosMedicos[key]);
      }
    }

    this.setValidators();
  }

  private carregarInscricoes() {
    this.inscricoesService.getAthleteSubscriptions(this.cpfAtleta).subscribe(
      (inscricoes) => {
        this.inscricoesAtleta = inscricoes;

        if (this.inscricoesAtleta.length > 0) {
          this.semInscricoes = false;
        } else {
          this.semInscricoes = true;
        }

        this.dsInscricoes = new MatTableDataSource(this.inscricoesAtleta);
      },
      (error) => {
        if (error.status == 401) {
          this.securityService.logOutToken();
        }
      }
    );
  }

  editarInscricao(idInscricao: number) {
    this.dialog.open(AtualizacaoInscricaoComponent, {
      data: { idInscricao: idInscricao, atleta: this.atleta },
    });
  }

  abrirInscricoes(eventoId: number) {
    this.router.navigateByUrl('eventos/inscritos/' + eventoId);
  }

  setValidators() {
    if (this.medicalControl.get('plano')!.value! == true) {
      this.medicalControl
        .get('planoEmpresa')!
        .setValidators(Validators.required);
      this.medicalControl.get('planoTipo')!.setValidators(Validators.required);
    } else {
      this.medicalControl.get('planoEmpresa')!.clearValidators();
      this.medicalControl.get('planoTipo')!.clearValidators();
    }

    if (this.medicalControl.get('cirurgia')?.value == true) {
      this.medicalControl
        .get('cirurgiaQual')!
        .setValidators(Validators.required);
    } else {
      this.medicalControl.get('cirurgiaQual')!.clearValidators();
    }

    if (this.medicalControl.get('medicacao')?.value == true) {
      this.medicalControl
        .get('medicacaoQual')!
        .setValidators(Validators.required);
      this.medicalControl
        .get('medicacaoTempo')!
        .setValidators(Validators.required);
    } else {
      this.medicalControl.get('medicacaoQual')!.clearValidators();
      this.medicalControl.get('medicacaoTempo')!.clearValidators();
    }

    if (this.medicalControl.get('malestar')?.value == true) {
      this.medicalControl
        .get('malestarQual')!
        .setValidators(Validators.required);
    } else {
      this.medicalControl.get('malestarQual')!.clearValidators();
    }

    if (this.medicalControl.get('acompanhamento')?.value == true) {
      this.medicalControl
        .get('acompanhamentoQual')!
        .setValidators(Validators.required);
    } else {
      this.medicalControl.get('acompanhamentoQual')!.clearValidators();
    }

    if (this.medicalControl.get('alergia')?.value == true) {
      this.medicalControl
        .get('alergiaQual')!
        .setValidators(Validators.required);
    } else {
      this.medicalControl.get('alergiaQual')!.clearValidators();
    }

    this.medicalControl.get('planoEmpresa')!.updateValueAndValidity();
    this.medicalControl.get('planoTipo')!.updateValueAndValidity();
    this.medicalControl.get('cirurgiaQual')!.updateValueAndValidity();
    this.medicalControl.get('medicacaoQual')!.updateValueAndValidity();
    this.medicalControl.get('medicacaoTempo')!.updateValueAndValidity();
    this.medicalControl.get('malestarQual')!.updateValueAndValidity();
    this.medicalControl.get('acompanhamentoQual')!.updateValueAndValidity();
    this.medicalControl.get('alergiaQual')!.updateValueAndValidity();
  }

  validaIdade() {
    let nascimento = this.athleteControl.get('nascimento')?.value?.toString();
    let dtNascimento = new Date().getFullYear();

    if (nascimento != null && nascimento != '') {
      dtNascimento = new Date(nascimento).getFullYear();
    }

    let idade = moment().diff(nascimento, 'years');

    if (idade < 18)
      this.athleteControl
        .get('responsavel')
        ?.setValidators(Validators.required);
    else this.athleteControl.get('responsavel')?.clearValidators();

    this.athleteControl.get('responsavel')?.updateValueAndValidity();
  }

  carregarEndereco(cep: string) {
    cep = cep.replace('-', '');

    if (cep.length < 8) return;

    this.externalServices.consultarEndereçoPorCEP(cep).subscribe((adress) => {
      this.athleteControl.get('endereco')?.setValue(adress.logradouro);
      this.athleteControl.get('cidade')?.setValue(adress.localidade);
      this.athleteControl.get('uf')?.setValue(adress.uf);

      this.athleteControl.get('numero')?.reset();
    });
  }

  salvarCadastro() {
    this.salvarDadosAtleta();
  }

  private salvarDadosAtleta() {
    this.atualizarClasseAtleta();
    this.atualizarClasseDadosMedicos();

    var atletaAtualizado: AtualizaAtleta = {
      nome: this.athleteControl.get('nome')?.value!,
      nascimento: this.athleteControl.get('nascimento')?.value!,
      sexo: this.athleteControl.get('sexo')?.value!,
      cpf: this.athleteControl.get('cpf')?.value!,
      rg: this.athleteControl.get('rg')?.value!,
      responsavel: this.athleteControl.get('responsavel')?.value
        ? this.athleteControl.get('responsavel')?.value!
        : '',
      endereco: this.athleteControl.get('endereco')?.value!,
      numero: this.athleteControl.get('numero')?.value!,
      complemento: this.athleteControl.get('complemento')?.value
        ? this.athleteControl.get('complemento')?.value!
        : '',
      cep: this.athleteControl.get('cep')?.value!,
      cidade: this.athleteControl.get('cidade')?.value!,
      uf: this.athleteControl.get('uf')?.value!,
      pais: this.athleteControl.get('pais')?.value!,
      telefone: this.athleteControl.get('telefone')?.value!
        ? this.athleteControl.get('telefone')?.value!
        : '',
      celular: this.athleteControl.get('celular')?.value!,
      email: this.athleteControl.get('email')?.value!,
      profissao: this.athleteControl.get('profissao')?.value
        ? this.athleteControl.get('profissao')?.value!
        : '',
      emergenciaContato: this.athleteControl.get('emergenciaContato')?.value!,
      emergenciaFone: this.athleteControl.get('emergenciaFone')?.value
        ? this.athleteControl.get('emergenciaFone')?.value!
        : '',
      emergenciaCelular: this.athleteControl.get('emergenciaCelular')?.value!,
      camisa: this.athleteControl.get('camisa')?.value!,
      camisaCiclismo: this.athleteControl.get('camisaCiclismo')?.value!,
      mktLojaPreferida: this.athleteControl.get('mktLojaPreferida')?.value
        ? this.athleteControl.get('mktLojaPreferida')?.value!
        : '',
      mktBikePreferida: this.athleteControl.get('mktBikePreferida')?.value
        ? this.athleteControl.get('mktBikePreferida')?.value!
        : '',
      mktAro: this.athleteControl.get('mktAro')?.value
        ? this.athleteControl.get('mktAro')?.value!
        : '',
      mktCambio: this.athleteControl.get('mktCambio')?.value
        ? this.athleteControl.get('mktCambio')?.value!
        : '',
      mktFreio: this.athleteControl.get('mktFreio')?.value
        ? this.athleteControl.get('mktFreio')?.value!
        : '',
      mktSuspensao: this.athleteControl.get('mktSuspensao')?.value
        ? this.athleteControl.get('mktSuspensao')?.value!
        : '',
      mktMarcaPneu: this.athleteControl.get('mktMarcaPneu')?.value
        ? this.athleteControl.get('mktMarcaPneu')?.value!
        : '',
      mktModeloPneu: this.athleteControl.get('mktModeloPneu')?.value
        ? this.athleteControl.get('mktModeloPneu')?.value!
        : '',
      mktTenis: this.athleteControl.get('mktTenis')?.value
        ? this.athleteControl.get('mktTenis')?.value!
        : '',
      federacao: this.athleteControl.get('federacao')?.value
        ? this.athleteControl.get('federacao')?.value!
        : '',
    };

    this.atletaService
      .atualizarAtleta(atletaAtualizado, this.idAtleta)
      .subscribe(
        () => {
          this.medicalService
            .atualizarDadosMedicos(this.dadosMedicos)
            .subscribe(
              () => {
                this.abrirMensagem('Cadastro atualizado');
              },
              (error) => {
                this.abrirMensagem('Ocorreu um erro ao atualizar o cadastro');
              }
            );
        },
        (error) => {
          this.abrirMensagem('Ocorreu um erro ao atualizar o cadastro');
        }
      );
  }

  private atualizarClasseAtleta() {
    this.atleta.nome = this.athleteControl.get('nome')?.value!;
    this.atleta.nascimento = this.athleteControl.get('nascimento')?.value!;
    this.atleta.sexo = this.athleteControl.get('sexo')?.value!;
    this.atleta.responsavel = this.athleteControl.get('responsavel')?.value
      ? this.athleteControl.get('responsavel')?.value!
      : '';
    this.atleta.endereco = this.athleteControl.get('endereco')?.value!;
    this.atleta.numero = this.athleteControl.get('numero')?.value!;
    this.atleta.complemento = this.athleteControl.get('complemento')?.value
      ? this.athleteControl.get('complemento')?.value!
      : '';
    this.atleta.cep = this.athleteControl.get('cep')?.value!;
    this.atleta.cidade = this.athleteControl.get('cidade')?.value!;
    this.atleta.uf = this.athleteControl.get('uf')?.value!;
    this.atleta.pais = this.athleteControl.get('pais')?.value!;
    this.atleta.telefone = this.athleteControl.get('telefone')?.value
      ? this.athleteControl.get('telefone')?.value!
      : '';
    this.atleta.celular = this.athleteControl.get('celular')?.value
      ? this.athleteControl.get('celular')?.value!
      : '';
    this.atleta.email = this.athleteControl.get('email')?.value!;
    this.atleta.profissao = this.athleteControl.get('profissao')?.value
      ? this.athleteControl.get('profissao')?.value!
      : '';
    this.atleta.emergenciaContato =
      this.athleteControl.get('emergenciaContato')?.value!;
    this.atleta.emergenciaFone = this.athleteControl.get('emergenciaFone')
      ?.value
      ? this.athleteControl.get('emergenciaFone')?.value!
      : '';
    this.atleta.emergenciaCelular =
      this.athleteControl.get('emergenciaCelular')?.value!;
    this.atleta.dataCadastro = this.athleteControl.get('dataCadastro')?.value!;
    this.atleta.camisa = this.athleteControl.get('camisa')?.value
      ? this.athleteControl.get('camisa')?.value!
      : '';
    this.atleta.camisaCiclismo = this.athleteControl.get('camisaCiclismo')
      ?.value
      ? this.athleteControl.get('camisaCiclismo')?.value!
      : '';
    this.atleta.mktLojaPreferida = this.athleteControl.get('mktLojaPreferida')
      ?.value
      ? this.athleteControl.get('mktLojaPreferida')?.value!
      : '';
    this.atleta.mktBikePreferida = this.athleteControl.get('mktBikePreferida')
      ?.value
      ? this.athleteControl.get('mktBikePreferida')?.value!
      : '';
    this.atleta.mktAro = this.athleteControl.get('mktAro')?.value
      ? this.athleteControl.get('mktAro')?.value!
      : '';
    this.atleta.mktCambio = this.athleteControl.get('mktCambio')?.value
      ? this.athleteControl.get('mktCambio')?.value!
      : '';
    this.atleta.mktFreio = this.athleteControl.get('mktFreio')?.value
      ? this.athleteControl.get('mktFreio')?.value!
      : '';
    this.atleta.mktSuspensao = this.athleteControl.get('mktSuspensao')?.value
      ? this.athleteControl.get('mktSuspensao')?.value!
      : '';
    this.atleta.mktMarcaPneu = this.athleteControl.get('mktMarcaPneu')?.value
      ? this.athleteControl.get('mktMarcaPneu')?.value!
      : '';
    this.atleta.mktModeloPneu = this.athleteControl.get('mktModeloPneu')?.value
      ? this.athleteControl.get('mktModeloPneu')?.value!
      : '';
    this.atleta.mktTenis = this.athleteControl.get('mktTenis')?.value
      ? this.athleteControl.get('mktTenis')?.value!
      : '';
    this.atleta.federacao = this.athleteControl.get('federacao')?.value
      ? this.athleteControl.get('federacao')?.value!
      : '';
    this.atleta.cpf = this.athleteControl.get('cpf')?.value!;
    this.atleta.rg = this.athleteControl.get('rg')?.value!;
  }

  atualizarClasseDadosMedicos() {
    this.dadosMedicos.plano = Number(this.medicalControl.get('plano')?.value!);
    this.dadosMedicos.planoEmpresa = this.medicalControl.get('planoEmpresa')
      ?.value
      ? this.medicalControl.get('planoEmpresa')?.value!
      : '';
    this.dadosMedicos.planoTipo = this.medicalControl.get('planoTipo')?.value
      ? this.medicalControl.get('planoTipo')?.value!
      : '';
    this.dadosMedicos.pressaoAlta = Number(
      this.medicalControl.get('pressaoalta')?.value!
    );
    this.dadosMedicos.desmaio = Number(
      this.medicalControl.get('desmaio')?.value!
    );
    this.dadosMedicos.cardiaco = Number(
      this.medicalControl.get('cardiaco')?.value!
    );
    this.dadosMedicos.diabetes = Number(
      this.medicalControl.get('diabetes')?.value!
    );
    this.dadosMedicos.asma = Number(this.medicalControl.get('asma')?.value!);
    this.dadosMedicos.alergia = Number(
      this.medicalControl.get('alergia')?.value!
    );
    this.dadosMedicos.alergiaQual = this.medicalControl.get('alergiaQual')
      ?.value
      ? this.medicalControl.get('alergiaQual')?.value!
      : '';
    this.dadosMedicos.cirurgiaQual = this.medicalControl.get('alergiaQual')
      ?.value
      ? this.medicalControl.get('cirurgiaQual')?.value!
      : '';
    this.dadosMedicos.cirurgia = Number(
      this.medicalControl.get('cirurgia')?.value!
    );
    this.dadosMedicos.cirurgiaQual = this.medicalControl.get('cirurgiaQual')
      ?.value
      ? this.medicalControl.get('cirurgiaQual')?.value!
      : '';
    this.dadosMedicos.medicacao = Number(
      this.medicalControl.get('medicacao')?.value!
    );
    this.dadosMedicos.medicacaoQual = this.medicalControl.get('medicacaoQual')
      ?.value
      ? this.medicalControl.get('medicacaoQual')?.value!
      : '';
    this.dadosMedicos.medicacaoTempo = this.medicalControl.get('medicacaoTempo')
      ?.value
      ? this.medicalControl.get('medicacaoTempo')?.value!
      : '';
    this.dadosMedicos.malestar = Number(
      this.medicalControl.get('malestar')?.value!
    );
    this.dadosMedicos.malestarQual = this.medicalControl.get('malestarQual')
      ?.value
      ? this.medicalControl.get('malestarQual')?.value!
      : '';
    this.dadosMedicos.acompanhamento = Number(
      this.medicalControl.get('acompanhamento')?.value!
    );
    this.dadosMedicos.acompanhamentoQual = this.medicalControl.get(
      'acompanhamentoQual'
    )?.value
      ? this.medicalControl.get('acompanhamentoQual')?.value!
      : '';
    this.dadosMedicos.outros = this.medicalControl.get('outros')?.value
      ? this.medicalControl.get('outros')?.value!
      : '';
  }

  visualizarBoleto(linkBoleto: string) {
    window.open(linkBoleto, '_blank');
  }

  novoPagamento(inscricao: Subscription) {
    this.eventosService
      .consultarEventoPeloId(inscricao.idEvento)
      .subscribe((retorno) => {
        this.abrirFormasPagamento(inscricao, retorno);
      });
  }

  private abrirFormasPagamento(inscricao: Subscription, evento: Evento) {
    this.eventosService
      .consultarValorPacote(inscricao.id!)
      .subscribe((retorno) => {
        console.log(retorno.valor);
        this.dialog.open(FormasPagamentosComponent, {
          data: {
            idInscricao: inscricao.id!,
            atleta: this.atleta,
            evento: evento,
            valorPagamento: Number(retorno.valor),
          },
          disableClose: true, // Isso impede que o usuário feche o diálogo clicando fora dele
        });
      });
  }

  salvarNovaSenha() {
    this.validandoSenha = true;
    if (
      this.senhaControl.get('novaSenha')?.value !=
      this.senhaControl.get('confirmarSenha')?.value
    ) {
      this.abrirMensagem('As senhas não batem');
      return;
    }

    var novaSenha: NovaSenha = {
      cpf: this.atleta.cpf,
      senhaAtual: this.senhaControl.get('senhaAtual')?.value!,
      novaSenha: this.senhaControl.get('novaSenha')?.value!,
    };

    this.atletaService.atualizarSenha(novaSenha).subscribe(() => {
      this.validandoSenha = false;

      this.abrirMensagem('Senha atualizada com sucesso');

      this.senhaControl.get('senhaAtual')?.reset();
      this.senhaControl.get('novaSenha')?.reset();
      this.senhaControl.get('confirmarSenha')?.reset();
    });
  }

  private abrirMensagem(message: string) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
    });
  }
}
