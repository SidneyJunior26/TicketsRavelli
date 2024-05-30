import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventRegisterComponent } from './Eventos/event-register/event-register.component';
import { EventsAllActiveListComponent } from './Eventos/events-all-active-list/events-all-active-list.component';
import { EventsManagerComponent } from './manager/painel/painel.component';
import { CadastroAtletaComponent } from './Atletas/cadastro-atleta/cadastro-atleta.component';
import { SenhaPrimeiroAcessoComponent } from './Atletas/senha-primeiro-acesso/senha-primeiro-acesso/senha-primeiro-acesso.component';
import { InscritosComponent } from './Eventos/inscritos/inscritos.component';
import { EsqueciSenhaComponent } from './Atletas/esqueci-senha/esqueci-senha.component';
import { FormasPagamentosComponent } from './Pagamentos/formas-pagamentos/formas-pagamentos.component';

const routes: Routes = [
  {
    path: '',
    component: EventsAllActiveListComponent,
  },
  {
    path: 'esqueci-minha-senha',
    component: EsqueciSenhaComponent,
  },
  {
    path: 'pagamento',
    component: FormasPagamentosComponent,
  },
  {
    path: ':idAfiliado',
    component: EventsAllActiveListComponent,
  },
  {
    path: 'eventos',
    children: [
      {
        path: ':idEvent',
        component: EventRegisterComponent,
      },
      {
        path: ':idEvent',
        component: EventRegisterComponent,
      },
      {
        path: 'inscritos/:idEvento',
        component: InscritosComponent,
      },
    ],
  },
  {
    path: 'cadastro',
    children: [
      {
        path: 'atleta',
        component: CadastroAtletaComponent,
      },
      {
        path: 'inscricoes',
        component: CadastroAtletaComponent,
      },
    ],
  },
  {
    path: 'manager',
    children: [
      {
        path: 'eventos',
        component: EventsManagerComponent,
      },
    ],
  },
  {
    path: 'nova-senha/:id',
    component: SenhaPrimeiroAcessoComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
