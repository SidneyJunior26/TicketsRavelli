import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCommonModule } from '@angular/material/core';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';
import { MatStepperModule } from '@angular/material/stepper';
import { MatIconModule } from '@angular/material/icon';
import { MatExpansionModule } from '@angular/material/expansion';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTooltipModule } from '@angular/material/tooltip';
import { EventRegisterComponent } from '../Eventos/event-register/event-register.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CadastroAtletaComponent } from './cadastro-atleta/cadastro-atleta.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatMenuModule } from '@angular/material/menu';
import { AtualizacaoInscricaoComponent } from './atualizacao-inscricao/atualizacao-inscricao.component';
import { SenhaPrimeiroAcessoComponent } from './senha-primeiro-acesso/senha-primeiro-acesso/senha-primeiro-acesso.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { EsqueciSenhaComponent } from './esqueci-senha/esqueci-senha.component';
import { NgxMaskModule } from 'ngx-mask';

@NgModule({
  declarations: [
    EventRegisterComponent,
    CadastroAtletaComponent,
    AtualizacaoInscricaoComponent,
    SenhaPrimeiroAcessoComponent,
    EsqueciSenhaComponent,
  ],
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatDatepickerModule,
    MatCommonModule,
    MatRadioModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatStepperModule,
    MatIconModule,
    MatExpansionModule,
    BrowserAnimationsModule,
    MatTooltipModule,
    MatCheckboxModule,
    MatTabsModule,
    MatTableModule,
    MatPaginatorModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    FormsModule,
    NgxMaskModule.forRoot(),
  ],
})
export class AthletesModule {}
