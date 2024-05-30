import {
  Component,
  HostListener,
  Inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { AtletasService } from 'src/app/core/Atletas/atletas.service';
import { EventosService } from 'src/app/core/Eventos/events.service';
import { RegulamentoService } from 'src/app/core/Regulamento/regulatmento.service';
import { SecurityService } from 'src/app/core/Segurança/security.service';
import { ExternalService } from 'src/app/core/Shared/external.service';
import { CategoriesService } from 'src/app/core/Categorias/categories-service.service';
import { LoginComponent } from 'src/app/shared/login/login.component';
import { Atleta } from 'src/app/shared/models/Atleta/atleta';
import { Evento } from 'src/app/shared/models/evento';
import { DadosMedicos } from 'src/app/shared/models/dadosMedicos';
import { Regulamento } from 'src/app/shared/models/regulamento';
import { NavbarComponent } from 'src/app/shared/navbar/navbar.component';
import { Pacotes } from 'src/app/shared/models/pacotes';
import { ViewportScroller } from '@angular/common';
import { Observable } from 'rxjs/internal/Observable';
import { CortesiaService } from 'src/app/core/Cortesia/cortesia.service';
import { FormasPagamentosComponent } from 'src/app/Pagamentos/formas-pagamentos/formas-pagamentos.component';
import { CupomDescontoService } from 'src/app/core/Cupons/cupom.service';
import { AtualizaAtleta } from 'src/app/shared/models/Atleta/atualizar-atleta';
import { Subscription } from 'src/app/shared/models/inscricao';
import { SubscriptionsService } from 'src/app/core/Inscricoes/inscricoes.service';
import { DadosMedicosService } from 'src/app/core/RegistrosMedicos/dados-medicos.service';
import { MatStepper } from '@angular/material/stepper';
import { MensagemModalComponent } from 'src/app/shared/mensagem-modal/mensagem-modal.component';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import 'moment/locale/fr';

const MY_DATE_FORMAT = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-event-register',
  templateUrl: './event-register.component.html',
  styleUrls: ['./event-register.component.css'],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMAT },
  ],
})
export class EventRegisterComponent implements OnInit {
  isDesktop: boolean = window.innerWidth < 1025;

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.isDesktop = event.target.innerWidth < 1025;
  }

  idEvento: number;
  cpfAtleta: string;
  atletaId: string;
  event: Evento;
  atleta: Atleta;
  pacotes: Pacotes;
  regulation: Regulamento;
  registroMedico: DadosMedicos;

  loadingAtleta: boolean = true;
  loadingRegistrosMedicos: boolean = true;
  loadingEvento: boolean = true;
  loadingRegulamento: boolean = true;
  loadingCep: boolean = false;
  loadingInscricao: boolean = false;
  loadingCategoria: boolean = false;
  loadingPacotes: boolean = false;
  loadingDesconto: boolean = false;

  cadastrandoAtleta: boolean = false;

  categorySelected = false;
  regulamentoAceito = false;
  token: string;
  categories = new FormControl('');
  categorieList: string[] = [];
  cuponsUtilizados: string[] = [];
  orientacao: Observable<string>;
  subCategorieList: [
    { id: string; name: string; aviso: string; dupla: number }
  ] = [{ id: '', name: '', aviso: '', dupla: 0 }];

  semNumero: boolean = false;
  cupomDesconto: string = '';
  cupomDescontoAplicado: string = '';
  cupomCortesia: string = '';
  valorPacote: number;
  valorCortesia: number = 0;
  porcentagemDesconto: number = 0;
  valorDesconto: number = 0;
  valorTotal: number;
  valorTaxas: number = 0;
  kitCamisa: boolean = false;
  kitCamisaPoliamida: boolean = false;
  categoriaDupla: boolean = false;
  categoriaEquipe: boolean = false;

  camisa: string;
  camisaCiclismo: string;

  pacoteSelecionado: string;

  cortesia: boolean = false;

  constructor(
    private securityService: SecurityService,
    private eventosService: EventosService,
    private atletasService: AtletasService,
    private dadosMedicosService: DadosMedicosService,
    private subCategoriasService: CategoriesService,
    private regulationService: RegulamentoService,
    private cortesiaService: CortesiaService,
    private cupomDescontoService: CupomDescontoService,
    private inscricaoService: SubscriptionsService,
    private externalServices: ExternalService,
    private activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private navBar: NavbarComponent,
    private scroll: ViewportScroller,
    private router: Router,
    private dateAdapter: DateAdapter<any>,
    @Inject(MAT_DATE_LOCALE) private _locale: string
  ) {
    this.pacoteSelecionado = '1';
  }

  eventControl = this.formBuilder.group({
    categoria: new FormControl('', Validators.required),
    subCategoria: new FormControl('', Validators.required),
    pacote: new FormControl(1, Validators.required),
    dupla: new FormControl(''),
    equipe: new FormControl(''),
  });

  atletaControl = this.formBuilder.group({
    nome: new FormControl('', Validators.required),
    nascimento: new FormControl('', Validators.required),
    sexo: new FormControl('1', Validators.required),
    cpf: new FormControl('', [Validators.required, Validators.minLength(11)]),
    rg: new FormControl('', Validators.required),
    responsavel: new FormControl(''),
    endereco: new FormControl('', Validators.required),
    numero: new FormControl('', Validators.required),
    complemento: new FormControl(''),
    cep: new FormControl('', Validators.required),
    cidade: new FormControl('', Validators.required),
    uf: new FormControl('', [Validators.required, Validators.minLength(2)]),
    pais: new FormControl('', Validators.required),
    telefone: new FormControl(''),
    celular: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.required]),
    profissao: new FormControl(''),
    emergenciaContato: new FormControl('', Validators.required),
    emergenciaTelefone: new FormControl(''),
    emergenciaCelular: new FormControl('', Validators.required),
    camisa: new FormControl(''),
    camisaCiclismo: new FormControl(''),
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
    senha: new FormControl('', [Validators.required, Validators.minLength(8)]),
    confirmaSenha: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
  });

  medicalControl = this.formBuilder.group({
    plano: new FormControl(null, Validators.required),
    planoEmpresa: new FormControl(''),
    planoTipo: new FormControl(''),
    pressaoalta: new FormControl(null, Validators.required),
    desmaio: new FormControl(null, Validators.required),
    cardiaco: new FormControl(null, Validators.required),
    diabetes: new FormControl(null, Validators.required),
    asma: new FormControl(null, Validators.required),
    alergia: new FormControl(null, Validators.required),
    alergiaQual: new FormControl(''),
    cirurgia: new FormControl(null, Validators.required),
    cirurgiaQual: new FormControl(''),
    medicacao: new FormControl(null, Validators.required),
    medicacaoQual: new FormControl(''),
    medicacaoTempo: new FormControl(''),
    malestar: new FormControl(null, Validators.required),
    malestarQual: new FormControl(''),
    acompanhamento: new FormControl(null, Validators.required),
    acompanhamentoQual: new FormControl(''),
    outros: new FormControl(''),
  });

  pagamentoControl = this.formBuilder.group({
    pacote: new FormControl('', Validators.required),
  });

  estadosBrasileiros = [
    { sigla: 'AC', nome: 'Acre' },
    { sigla: 'AL', nome: 'Alagoas' },
    { sigla: 'AP', nome: 'Amapá' },
    { sigla: 'AM', nome: 'Amazonas' },
    { sigla: 'BA', nome: 'Bahia' },
    { sigla: 'CE', nome: 'Ceará' },
    { sigla: 'DF', nome: 'Distrito Federal' },
    { sigla: 'ES', nome: 'Espírito Santo' },
    { sigla: 'GO', nome: 'Goiás' },
    { sigla: 'MA', nome: 'Maranhão' },
    { sigla: 'MT', nome: 'Mato Grosso' },
    { sigla: 'MS', nome: 'Mato Grosso do Sul' },
    { sigla: 'MG', nome: 'Minas Gerais' },
    { sigla: 'PA', nome: 'Pará' },
    { sigla: 'PB', nome: 'Paraíba' },
    { sigla: 'PR', nome: 'Paraná' },
    { sigla: 'PE', nome: 'Pernambuco' },
    { sigla: 'PI', nome: 'Piauí' },
    { sigla: 'RJ', nome: 'Rio de Janeiro' },
    { sigla: 'RN', nome: 'Rio Grande do Norte' },
    { sigla: 'RS', nome: 'Rio Grande do Sul' },
    { sigla: 'RO', nome: 'Rondônia' },
    { sigla: 'RR', nome: 'Roraima' },
    { sigla: 'SC', nome: 'Santa Catarina' },
    { sigla: 'SP', nome: 'São Paulo' },
    { sigla: 'SE', nome: 'Sergipe' },
    { sigla: 'TO', nome: 'Tocantins' },
  ];

  @ViewChild('stepper') stepper: MatStepper;

  ngOnInit(): void {
    this.idEvento = this.activatedRoute.snapshot.params['idEvent'];

    this.token = this.securityService.getToken();

    if (this.token != '') {
      var userInfo = this.securityService.getDecodedAccessToken(this.token);
    }
    if (userInfo != null) {
      this.navBar.checkLogIn();
      this.atletaId = userInfo.ID;
      this.cpfAtleta = userInfo.cpf;
      this.verificaAtletaInscrito(this.cpfAtleta, this.idEvento);
    } else {
      this.loadingAtleta = false;
      this.loadingRegistrosMedicos = false;
    }

    this.setValidatorsSenha();

    this.carregarEvento();
    this.carregarRegulamento();

    this.scroll.scrollToPosition([0, 0]);
  }

  ngOnDestroy() {
    localStorage.removeItem('eventId');
    localStorage.removeItem('cpf');
  }

  private verificaAtletaInscrito(cpfAtleta: string, eventId: number) {
    this.inscricaoService
      .checkIfAthleteSubscribedByEvent(cpfAtleta, eventId)
      .subscribe(
        () => {
          this.abrirMensagem('Você já esta inscrito neste evento', false);
          this.router.navigateByUrl('/');
        },
        (error) => {
          if (error.status == 404) this.consultarAtleta();
        }
      );
  }

  verificarUsuarioJaExisteCPF() {
    var cpf = this.atletaControl.get('cpf')!.value!.toString();

    if (this.atletaControl.get('cpf')!.value == null) return;

    if (this.atleta != null) {
      if (this.atleta.cpf == cpf) return;
    }

    this.atletasService.verificarUsuarioExiste(cpf).subscribe(() => {
      this.dialog
        .open(MensagemModalComponent, {
          width: '600px',
          data: {
            mensagem: 'CPF já cadastrado.\nRealize o login.',
            icone: 'warning',
          },
        })
        .afterClosed()
        .subscribe(() => {
          this.logIn(cpf, this.idEvento.toString());
        });

      this.atletaControl.get('cpf')!.setValue('');
    });
  }

  verificarUsuarioJaExisteEmail() {
    if (this.atletaControl.get('email')!.value == null) return;

    var email = this.atletaControl.get('email')!.value!.toString();

    if (this.atleta != null) {
      if (this.atleta.email == email) return;
    }

    this.atletasService.verificarUsuarioExisteEmail(email).subscribe(() => {
      this.dialog.open(MensagemModalComponent, {
        width: '600px',
        data: {
          mensagem:
            'Email já cadastrado em nosso sistema.\nFavor informar outro',
          icone: 'warning',
        },
      });

      this.atletaControl.get('email')!.setValue('');
    });
  }

  verificarUsuarioJaExisteRG() {
    if (this.atletaControl.get('rg')!.value == null) return;

    var rg = this.atletaControl.get('rg')!.value!.toString();

    if (this.atleta != null) {
      if (this.atleta.rg == rg) return;
    }

    this.atletasService.verificarUsuarioExisteRG(rg).subscribe(() => {
      this.dialog.open(MensagemModalComponent, {
        width: '600px',
        data: {
          mensagem: 'RG já cadastrado em nosso sistema.',
          icone: 'warning',
        },
      });

      this.atletaControl.get('rg')!.setValue('');
    });
  }

  private logIn(cpf: string, eventId: string) {
    localStorage.setItem('eventId', eventId);
    localStorage.setItem('cpf', cpf);
    this.dialog.open(LoginComponent);
  }

  cadastrarAtleta(): void {
    this.cadastrandoAtleta = true;

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
      endereco: this.atletaControl.get('endereco')!.value!.toString(),
      numero: this.atletaControl.get('numero')!.value!.toString(),
      complemento:
        this.atletaControl.get('complemento')!.value != null
          ? this.atletaControl.get('complemento')!.value!.toString()
          : '',
      cep: this.atletaControl.get('cep')!.value!.toString(),
      cidade: this.atletaControl.get('cidade')!.value!.toString(),
      uf: this.atletaControl.get('uf')!.value!.toString(),
      pais: this.atletaControl.get('pais')!.value!.toString(),
      telefone:
        this.atletaControl.get('telefone')!.value != null
          ? this.atletaControl.get('telefone')!.value!.toString()
          : '',
      celular: this.atletaControl.get('celular')!.value!.toString(),
      email: this.atletaControl.get('email')!.value!.toString(),
      profissao:
        this.atletaControl.get('profissao')!.value != null
          ? this.atletaControl.get('profissao')!.value!.toString()
          : '',
      emergenciaContato: this.atletaControl
        .get('emergenciaContato')!
        .value!.toString(),
      emergenciaFone:
        this.atletaControl.get('emergenciaTelefone')!.value != null
          ? this.atletaControl.get('emergenciaTelefone')!.value!.toString()
          : '',
      emergenciaCelular: this.atletaControl
        .get('emergenciaCelular')!
        .value!.toString(),
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

    this.atletasService.cadastrarAtleta(novoAtleta).subscribe(
      (retorno) => {
        this.atleta = novoAtleta;

        this.cadastrarDadosMedicos(retorno.idAtleta);

        this.cadastrandoAtleta = false;

        this.stepper.selectedIndex = 4;
      },
      (retornoError) => {
        this.cadastrandoAtleta = false;

        let message = '';

        // Verifica se há algum erro na propriedade 'errors'
        if (
          retornoError.error.errors &&
          Object.keys(retornoError.error.errors).length > 0
        ) {
          // Obtém a primeira chave (que é o nome do campo) e sua mensagem de erro associada
          const firstError = Object.keys(retornoError.error.errors)[0];
          message = retornoError.error.errors[firstError][0];
        } else if (retornoError.error.message != null) {
          message = retornoError.error.message;
        }

        var dialog = this.dialog.open(MensagemModalComponent, {
          width: '600px',
          data: {
            mensagem:
              message != ''
                ? message
                : 'Ocorreu um erro ao realizar seu cadastro. Revise seus dados',
            icone: 'warning',
          },
        });

        dialog.afterClosed().subscribe(() => {
          this.regulamentoAceito = false;
          this.stepper.selectedIndex = 0;
        });
      }
    );
  }

  private cadastrarDadosMedicos(idAtleta: string) {
    var dadosMedicos: DadosMedicos = {
      idAtleta: idAtleta,
      plano: this.medicalControl.get('plano')?.value!,
      planoEmpresa: this.medicalControl.get('planoEmpresa')?.value
        ? this.medicalControl.get('planoEmpresa')?.value!
        : '',
      planoTipo: this.medicalControl.get('planoTipo')?.value
        ? this.medicalControl.get('planoTipo')?.value!
        : '',
      pressaoAlta: this.medicalControl.get('pressaoalta')?.value!,
      desmaio: this.medicalControl.get('desmaio')?.value!,
      cardiaco: this.medicalControl.get('cardiaco')?.value!,
      diabetes: this.medicalControl.get('diabetes')?.value!,
      asma: this.medicalControl.get('asma')?.value!,
      alergia: this.medicalControl.get('alergia')?.value!,
      alergiaQual: this.medicalControl.get('alergiaQual')?.value
        ? this.medicalControl.get('alergiaQual')?.value!
        : '',
      cirurgia: this.medicalControl.get('cirurgia')?.value!,
      cirurgiaQual: this.medicalControl.get('cirurgiaQual')?.value
        ? this.medicalControl.get('cirurgiaQual')?.value!
        : '',
      medicacao: this.medicalControl.get('medicacao')?.value!,
      medicacaoQual: this.medicalControl.get('medicacaoQual')?.value
        ? this.medicalControl.get('medicacaoQual')?.value!
        : '',
      medicacaoTempo: this.medicalControl.get('medicacaoTempo')?.value
        ? this.medicalControl.get('medicacaoTempo')?.value!
        : '',
      malestar: this.medicalControl.get('malestar')?.value!,
      malestarQual: this.medicalControl.get('malestarQual')?.value
        ? this.medicalControl.get('malestarQual')?.value!
        : '',
      acompanhamento: this.medicalControl.get('acompanhamento')?.value!,
      acompanhamentoQual: this.medicalControl.get('acompanhamentoQual')?.value
        ? this.medicalControl.get('acompanhamentoQual')?.value!
        : '',
      outros: this.medicalControl.get('outros')?.value
        ? this.medicalControl.get('outros')?.value!
        : '',
    };

    this.dadosMedicosService
      .cadastrarDadosMedicos(dadosMedicos)
      .subscribe(() => {
        this.realizarLogin(this.atleta.cpf, this.atleta.acesso!);
      });
  }

  private realizarLogin(cpf: string, senha: string) {
    this.atletasService.login(cpf, senha).subscribe(
      (token) => {
        localStorage.setItem('currentUser', token);
        this.stepper.next();
      },
      (error) => {
        if (error.status == 404) {
          this.abrirMensagem('Usuário não encontrado');
        }
        if (error.status == 401) {
          this.abrirMensagem('Senha inválida');
        }
      }
    );
  }

  atualizarAtleta(): void {
    this.cadastrandoAtleta = true;

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

    this.atletasService
      .atualizarAtleta(atletaAtualizado, this.atleta.id!)
      .subscribe(
        () => {
          this.atualizarDadosMedicos();

          this.cadastrandoAtleta = false;

          this.stepper.selectedIndex = 4;
        },
        (retornoError) => {
          this.cadastrandoAtleta = false;

          let message = '';

          // Verifica se há algum erro na propriedade 'errors'
          if (
            retornoError.error.errors &&
            Object.keys(retornoError.error.errors).length > 0
          ) {
            // Obtém a primeira chave (que é o nome do campo) e sua mensagem de erro associada
            const primeiroErro = Object.keys(retornoError.error.errors)[0];
            message = retornoError.error.errors[primeiroErro][0];
          } else if (retornoError.error.message != null) {
            message = retornoError.error.message;
          }

          var dialog = this.dialog.open(MensagemModalComponent, {
            width: '600px',
            data: {
              mensagem:
                message != ''
                  ? message
                  : 'Ocorreu um erro ao atualizar seus dados. Por favor, revise seus dados',
              icone: 'warning',
            },
          });

          dialog.afterClosed().subscribe(() => {
            this.regulamentoAceito = false;
            this.stepper.selectedIndex = 0;
          });
        }
      );
  }

  private atualizarDadosMedicos() {
    var dadosMedicos: DadosMedicos = {
      idAtleta: '',
      plano: this.medicalControl.get('plano')?.value!,
      planoEmpresa: this.medicalControl.get('planoEmpresa')?.value
        ? this.medicalControl.get('planoEmpresa')?.value!
        : '',
      planoTipo: this.medicalControl.get('planoTipo')?.value
        ? this.medicalControl.get('planoTipo')?.value!
        : '',
      pressaoAlta: this.medicalControl.get('pressaoalta')?.value!,
      desmaio: this.medicalControl.get('desmaio')?.value!,
      cardiaco: this.medicalControl.get('cardiaco')?.value!,
      diabetes: this.medicalControl.get('diabetes')?.value!,
      asma: this.medicalControl.get('asma')?.value!,
      alergia: this.medicalControl.get('alergia')?.value!,
      alergiaQual: this.medicalControl.get('alergiaQual')?.value
        ? this.medicalControl.get('alergiaQual')?.value!
        : '',
      cirurgia: this.medicalControl.get('cirurgia')?.value!,
      cirurgiaQual: this.medicalControl.get('cirurgiaQual')?.value
        ? this.medicalControl.get('cirurgiaQual')?.value!
        : '',
      medicacao: this.medicalControl.get('medicacao')?.value!,
      medicacaoQual: this.medicalControl.get('medicacaoQual')?.value
        ? this.medicalControl.get('medicacaoQual')?.value!
        : '',
      medicacaoTempo: this.medicalControl.get('medicacaoTempo')?.value
        ? this.medicalControl.get('medicacaoTempo')?.value!
        : '',
      malestar: this.medicalControl.get('malestar')?.value!,
      malestarQual: this.medicalControl.get('malestarQual')?.value
        ? this.medicalControl.get('malestarQual')?.value!
        : '',
      acompanhamento: this.medicalControl.get('acompanhamento')?.value!,
      acompanhamentoQual: this.medicalControl.get('acompanhamentoQual')?.value
        ? this.medicalControl.get('acompanhamentoQual')?.value!
        : '',
      outros: this.medicalControl.get('outros')?.value
        ? this.medicalControl.get('outros')?.value!
        : '',
    };

    this.dadosMedicosService
      .atualizarDadosMedicos(dadosMedicos)
      .subscribe(() => {
        this.abrirMensagem('Seus dados foram atualizados');
      });
  }

  loadSubcategory() {
    this.loadingCategoria = true;

    this.eventControl.get('subCategoria')!.setValue('');

    let category = Number(this.eventControl.get('categoria')?.value!);
    let sexo = this.atletaControl.get('sexo')?.value!;
    let nascimento = new Date(
      this.atletaControl.get('nascimento')!.value!.toString()
    ).getFullYear();
    let dtNascimento = new Date().getFullYear();

    if (nascimento != null) {
      dtNascimento = new Date(nascimento).getFullYear();
    }

    let dataEvento = new Date(this.event.data).getFullYear();
    let idade = dataEvento - nascimento;

    this.subCategoriasService
      .consultarCategoriasFiltrado(
        this.event.id!,
        category,
        idade,
        Number(sexo)
      )
      .subscribe(
        (subCategories) => {
          this.loadingCategoria = false;

          this.subCategorieList = [{ id: '', name: '', aviso: '', dupla: 0 }];

          subCategories.forEach((item) => {
            this.subCategorieList.push({
              id: item.id!.toString(),
              name: item.descSubcategoria,
              aviso: item.aviso,
              dupla: item.filtroDupla,
            });
          });

          this.subCategorieList.splice(0, 1);
        },
        (error) => {
          this.loadingCategoria = false;
        }
      );
  }

  protected verificaCampoDupla(filtroDupla: number) {
    if (filtroDupla == 0) {
      this.categoriaDupla = false;
      this.categoriaEquipe = false;

      this.eventControl.get('dupla')?.clearValidators();
      this.eventControl.get('dupla')?.updateValueAndValidity();

      this.eventControl.get('equipe')?.clearValidators();
      this.eventControl.get('equipe')?.updateValueAndValidity();
    } else if (filtroDupla == 1) {
      this.categoriaDupla = true;
      this.categoriaEquipe = false;

      this.eventControl.get('dupla')?.addValidators(Validators.required);
      this.eventControl.get('dupla')?.updateValueAndValidity();

      this.eventControl.get('equipe')?.clearValidators();
      this.eventControl.get('equipe')?.updateValueAndValidity();
    } else {
      this.categoriaDupla = false;
      this.categoriaEquipe = true;

      this.eventControl.get('dupla')?.clearValidators();
      this.eventControl.get('dupla')?.updateValueAndValidity();

      this.eventControl.get('equipe')?.addValidators(Validators.required);
      this.eventControl.get('equipe')?.updateValueAndValidity();
    }
  }

  loadAdress(cep: string) {
    if (cep.length < 8) {
      this.loadingCep = false;
      return;
    }

    this.externalServices.consultarEndereçoPorCEP(cep).subscribe(
      (adress) => {
        this.atletaControl.get('endereco')?.setValue(adress.logradouro);
        this.atletaControl.get('cidade')?.setValue(adress.localidade);
        this.atletaControl.get('uf')?.setValue(adress.uf);
        this.loadingCep = false;
      },
      () => (this.loadingCep = false)
    );
  }

  private carregarEvento() {
    this.loadingEvento = true;
    this.eventosService.consultarEventoPeloId(this.idEvento).subscribe(
      (event) => {
        this.event = event;

        const categories = this.event.categoria.split(';');

        categories.forEach((item) => {
          this.categorieList.push(item);
        });
      },
      (error) => {
        this.abrirMensagem('Ocorreu um erro ao carregar os dados do evento');
      },
      () => {
        this.loadingEvento = false;
      }
    );
  }

  carregarValoresPacotes() {
    this.eventosService
      .consultarPacotesPorEvento(this.idEvento)
      .subscribe((pacotes) => {
        this.pacotes = pacotes;
        this.valorPacote = pacotes.valorPacote1;
        this.valorTotal = this.valorPacote + this.valorTaxas;

        this.verificaKit(pacotes.descricaoPacote1);
      });
  }

  private carregarRegulamento() {
    this.loadingRegulamento = true;
    this.regulationService
      .consultarRegulamentoPorEvento(this.idEvento)
      .subscribe(
        (regulation) => {
          this.regulation = regulation;

          this.carregarValoresPacotes();

          this.loadingRegulamento = false;
        },
        (error) => {
          this.loadingRegulamento = false;
        }
      );
  }

  private consultarAtleta() {
    this.loadingAtleta = true;
    this.loadingRegistrosMedicos = true;

    this.atletasService.consultarAtletaPorCPF(this.cpfAtleta).subscribe(
      (athlete) => {
        this.atleta = athlete;

        if (this.atleta != null) {
          this.carregarControlAtleta();
          this.registroMedico = this.atleta.registroMedico!;

          this.camisa = this.atleta.camisa;
          this.camisaCiclismo = this.atleta.camisaCiclismo;
        }

        if (this.atleta.registroMedico != null) {
          this.registroMedico = this.atleta.registroMedico!;

          this.carregarControlRegistrosMedicos();
        }
      },
      (error) => {
        if (error.status == 401) {
          this.securityService.logOutToken();
          localStorage.setItem('eventId', this.idEvento.toString());
          localStorage.setItem('cpf', this.cpfAtleta);
          this.dialog.open(LoginComponent);
        }
      },
      () => {
        this.loadingAtleta = false;
        this.setValidatorsSenha();
      }
    );
  }

  private carregarControlAtleta() {
    this.atletaControl.markAllAsTouched();

    for (const key in this.atleta) {
      if (this.atletaControl.get(key)) {
        this.atletaControl.get(key)?.setValue(this.atleta[key]);
        this.atletaControl.get(key)?.updateValueAndValidity();
      }
    }

    this.atletaControl.get('nome')?.setValue('');
  }

  private carregarControlRegistrosMedicos() {
    this.medicalControl.markAllAsTouched();

    for (const key in this.registroMedico) {
      if (this.medicalControl.get(key)) {
        this.medicalControl.get(key)?.setValue(this.registroMedico[key]);
        this.medicalControl.get(key)?.updateValueAndValidity();
      }
    }
    this.loadingRegistrosMedicos = false;
    this.setValidators();
  }

  private abrirMensagem(message: string, topPosition = true) {
    this.snackBar.open(message, 'OK', {
      duration: 2000,
      horizontalPosition: 'center',
      verticalPosition: topPosition ? 'top' : 'bottom',
    });
  }

  confirmaRegulamento() {
    if (this.regulamentoAceito) this.regulamentoAceito = false;
    else this.regulamentoAceito = true;
  }

  getDateFormatString(): string {
    if (this._locale === 'ja-JP') {
      return 'YYYY/MM/DD';
    } else if (this._locale === 'en-GB') {
      return 'DD/MM/YYYY';
    }
    return '';
  }

  validaIdade() {
    let nascimento = this.atletaControl.get('nascimento')?.value?.toString();
    let dtNascimento = new Date().getFullYear();

    if (nascimento != null && nascimento != '') {
      dtNascimento = new Date(nascimento).getFullYear();
    }

    let idade = moment().diff(nascimento, 'years');

    if (idade < 18)
      this.atletaControl.get('responsavel')?.setValidators(Validators.required);
    else this.atletaControl.get('responsavel')?.clearValidators();

    this.atletaControl.get('responsavel')?.updateValueAndValidity();

    this.dateAdapter.setLocale('en-GB');
  }

  private setValidatorsSenha() {
    if (this.atleta == null) {
      this.atletaControl
        .get('senha')!
        .setValidators([Validators.required, Validators.minLength(8)]);
      this.atletaControl
        .get('confirmaSenha')!
        .setValidators([Validators.required, Validators.minLength(8)]);
    } else {
      this.atletaControl.get('senha')!.clearValidators();
      this.atletaControl.get('confirmaSenha')!.clearValidators();
    }

    this.atletaControl.get('senha')!.updateValueAndValidity();
    this.atletaControl.get('confirmaSenha')!.updateValueAndValidity();
  }

  setValidators() {
    if (this.medicalControl.get('plano')?.value! == 1) {
      this.medicalControl
        .get('planoEmpresa')!
        .setValidators(Validators.required);
      this.medicalControl.get('planoTipo')!.setValidators(Validators.required);
    } else {
      this.medicalControl.get('planoEmpresa')!.clearValidators();
      this.medicalControl.get('planoTipo')!.clearValidators();
    }

    if (this.medicalControl.get('cirurgia')?.value! == 1) {
      this.medicalControl
        .get('cirurgiaQual')!
        .setValidators(Validators.required);
    } else {
      this.medicalControl.get('cirurgiaQual')!.clearValidators();
    }

    if (this.medicalControl.get('medicacao')?.value! == 1) {
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

    if (this.medicalControl.get('malestar')?.value! == 1) {
      this.medicalControl
        .get('malestarQual')!
        .setValidators(Validators.required);
    } else {
      this.medicalControl.get('malestarQual')!.clearValidators();
    }

    if (this.medicalControl.get('acompanhamento')?.value! == 1) {
      this.medicalControl
        .get('acompanhamentoQual')!
        .setValidators(Validators.required);
    } else {
      this.medicalControl.get('acompanhamentoQual')!.clearValidators();
    }

    if (this.medicalControl.get('alergia')?.value! == 1) {
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

  consultarCupomCortesia() {
    if (this.valorCortesia == 0)
      this.cortesiaService
        .consultarCupomCortesia(this.idEvento, this.cupomCortesia)
        .subscribe(
          () => {
            this.aplicarCortesia();

            this.cortesia = true;

            this.dialog.open(MensagemModalComponent, {
              width: '600px',
              data: {
                mensagem: 'Cupom cortesia aplicado com sucesso!',
                icone: 'check_circle',
              },
            });
          },
          (error) => {
            if (error.status == 404)
              this.dialog.open(MensagemModalComponent, {
                width: '600px',
                data: {
                  mensagem: error.error.message,
                  icone: 'check_circle',
                },
              });

            this.cortesia = false;
          }
        );
  }

  consultarCupomDesconto() {
    this.loadingDesconto = true;

    if (this.cuponsUtilizados.includes(this.cupomDesconto)) {
      this.abrirMensagem('Este cupom já foi utilizado');
      return;
    }

    if (this.valorDesconto == 0)
      this.cupomDescontoService
        .validarCupom(this.cupomDesconto, this.idEvento)
        .subscribe(
          (retorno) => {
            this.loadingDesconto = false;

            this.cuponsUtilizados.push(this.cupomDesconto);
            this.cupomDescontoAplicado = this.cupomDesconto;

            this.porcentagemDesconto = retorno.porcentagem;

            this.aplicarDesconto();

            this.abrirMensagem('Cupom validado');
          },
          (error) => {
            this.loadingDesconto = false;

            if (error.message) {
              this.abrirMensagem(error.error.message);

              this.cupomDesconto = '';
            }
          }
        );
  }

  realizarInscricao() {
    this.salvarTamanhoCamisetas();
    this.cadastrarInscricao();
  }

  private cadastrarInscricao() {
    this.loadingInscricao = true;

    var novaInscricao: Subscription = {
      idEvento: this.idEvento,
      cpfAtleta: this.atletaControl.get('cpf')?.value!,
      idSubcategoria: Number(this.eventControl.get('subCategoria')?.value),
      equipe: this.atletaControl.get('equipe')?.value
        ? this.atletaControl.get('equipe')?.value
        : '',
      dupla: this.eventControl.get('dupla')?.value
        ? this.eventControl.get('dupla')?.value!.toString()
        : '',
      quarteto: this.eventControl.get('equipe')?.value
        ? this.eventControl.get('equipe')?.value!
        : '',
      pacote: Number(this.pacoteSelecionado),
      aceiteRegulamento: this.regulamentoAceito,
    };

    if (this.valorTotal == 0) {
      novaInscricao.pago = true;
    }

    if (this.cortesia) {
      novaInscricao.gnStatus = 'CORTESIA';
    }

    const idAfiliado = localStorage.getItem('affiliateId');

    if (idAfiliado != null && idAfiliado != '')
      novaInscricao.afiliadoId = idAfiliado;

    this.inscricaoService.postSubscription(novaInscricao).subscribe(
      (idInscricao) => {
        if (this.valorTotal > 0) {
          this.escolherFormaPagamento(idInscricao);
        } else {
          this.abrirMensagem('Inscrito com sucesso!');
          this.router.navigateByUrl('cadastro/inscricoes');
        }

        localStorage.removeItem('affiliateId');

        this.loadingInscricao = false;
      },
      (retornoError) => {
        this.loadingInscricao = false;

        let mensagem = '';

        // Verifica se há algum erro na propriedade 'errors'
        if (
          retornoError.error.errors &&
          Object.keys(retornoError.error.errors).length > 0
        ) {
          // Obtém a primeira chave (que é o nome do campo) e sua mensagem de erro associada
          const primeiroErro = Object.keys(retornoError.error.errors)[0];
          mensagem = retornoError.error.errors[primeiroErro][0];
        } else if (retornoError.error.message != null) {
          mensagem = retornoError.error.message;
        }

        if (mensagem != '') {
          var dialog = this.dialog.open(MensagemModalComponent, {
            width: '600px',
            data: {
              mensagem: mensagem,
              icone: 'warning',
            },
          });

          dialog.afterClosed().subscribe(() => {
            this.stepper.selectedIndex = 2;
          });
        } else {
          this.abrirMensagem(
            'Ocorreu um erro ao realizar sua inscricao. Tente novamente ou contate nossa administração'
          );
        }
      }
    );
  }

  private escolherFormaPagamento(idInscricao: number) {
    this.dialog.open(FormasPagamentosComponent, {
      data: {
        idInscricao: idInscricao,
        atleta: this.atleta,
        evento: this.event,
        valorPagamento: this.valorTotal,
        cupomDesconto: this.cupomDescontoAplicado,
      },
      disableClose: true, // Isso impede que o usuário feche o diálogo clicando fora dele
    });
  }

  private salvarTamanhoCamisetas() {
    var cpf = this.atletaControl.get('cpf')?.value!;

    var camisa = this.camisa;
    var camisaCiclismo = this.camisaCiclismo;

    if (camisa == null || camisaCiclismo == null) {
      return;
    }

    this.atletasService
      .atualizarCamisasAtleta(cpf, camisa, camisaCiclismo)
      .subscribe();
  }

  private aplicarCortesia() {
    this.valorCortesia = this.valorTotal;

    this.cupomCortesia = '';

    this.calcularValorTotal();
  }

  private aplicarDesconto() {
    this.valorDesconto = this.valorTotal * (this.porcentagemDesconto / 100);

    this.cupomDesconto = '';

    this.calcularValorTotal();
  }

  private calcularValorTotal() {
    if (this.valorCortesia > 0) {
      this.valorPacote = 0;
      this.valorTotal = 0;
    }

    if (this.valorDesconto > 0) {
      this.valorTotal = this.valorTotal - this.valorDesconto;
    }
  }

  carregarValorPacote(valorPacote: number, descPacote: string) {
    this.loadingPacotes = true;

    this.valorPacote = valorPacote;
    this.valorTotal = this.valorPacote + this.valorTaxas;

    if (this.valorCortesia > 0) this.aplicarCortesia();

    this.verificaKit(descPacote);

    this.aplicarDesconto();

    this.loadingPacotes = false;
  }

  private verificaKit(descPacote: string) {
    var poliamida = descPacote.toUpperCase().includes('POLIAMIDA');
    var ciclismo = descPacote.toUpperCase().includes('CICLISMO');

    if (poliamida) {
      this.kitCamisaPoliamida = true;
    } else {
      this.kitCamisaPoliamida = false;
    }
    if (ciclismo) {
      this.kitCamisa = true;
    } else {
      this.kitCamisa = false;
    }
  }

  semNumeroChanged() {
    if (this.atletaControl.get('numero')?.disabled) {
      this.atletaControl.get('numero')?.setValue('');
      this.atletaControl.get('numero')?.enable();
    } else {
      this.atletaControl.get('numero')?.disable();
      this.atletaControl.get('numero')?.setValue('SN');
    }
  }
}
