import { LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { EventsManagerComponent } from './painel/painel.component';
import { MatStepperModule } from '@angular/material/stepper';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { ImageUploadComponent } from '../shared/image-upload/image-upload.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CategoriesComponent } from './categories/categories.component';
import { RegulamentosComponent } from './regulamentos/regulamentos.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from '@angular/material/dialog';
import { DadosMedicosComponent } from './dados-medicos/dados-medicos.component';
import { CuponsComponent } from './cupons/cupons.component';
import { MatMenuModule } from '@angular/material/menu';
import { CadastroInscricaoComponent } from './cadastro-inscricao/cadastro-inscricao/cadastro-inscricao.component';
import { CortesiaComponent } from './cortesia/cortesia.component';
import { RelatoriosComponent } from './relatorios/relatorios.component';
import { AtletaComponent } from './atleta/atleta.component';
import { EventoComponent } from './evento/evento.component';
import { EfetivarInscricaoComponent } from './efetivar-inscricao/efetivar-inscricao.component';
import { AlterarSenhaComponent } from './alterar-senha/alterar-senha.component';
import { InscricoesComponent } from './inscricoes/inscricoes.component';
import { AfiliadoComponent } from './afiliado/afiliado.component';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { CPFPipe } from '../shared/pipes/cpf.pipe';
import localePT from '@angular/common/locales/pt';
import { NgxMaskModule } from 'ngx-mask';
import { InscricoesAtletaComponent } from './inscricoes-atleta/inscricoes-atleta.component';
registerLocaleData(localePT);

@NgModule({
  declarations: [
    EventsManagerComponent,
    ImageUploadComponent,
    CategoriesComponent,
    RegulamentosComponent,
    DadosMedicosComponent,
    CuponsComponent,
    CadastroInscricaoComponent,
    CortesiaComponent,
    RelatoriosComponent,
    AtletaComponent,
    EventoComponent,
    EfetivarInscricaoComponent,
    AlterarSenhaComponent,
    InscricoesComponent,
    AfiliadoComponent,
    CPFPipe,
    InscricoesAtletaComponent,
  ],
  imports: [
    CommonModule,
    ClipboardModule,
    MatStepperModule,
    MatTabsModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatTooltipModule,
    MatPaginatorModule,
    MatDatepickerModule,
    MatSortModule,
    MatRadioModule,
    MatSelectModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatMenuModule,
    NgxMaskModule.forRoot(),
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'pt-br' }],
})
export class ManagerModule {}
