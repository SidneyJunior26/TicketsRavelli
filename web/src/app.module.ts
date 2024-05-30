import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { EventsModule } from './Eventos/events.module';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './shared/login/login.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { AthletesModule } from './Atletas/athletes.module';
import { ManagerModule } from './manager/manager.module';
import { MensagemConfirmacaoComponent } from './shared/mensagem-confirmacao/mensagem-confirmacao.component';
import { PagamentosModule } from './Pagamentos/pagamentos.module';
import { MatIconModule } from '@angular/material/icon';
import { MensagemModalComponent } from './shared/mensagem-modal/mensagem-modal.component';
import { NgxMaskModule } from 'ngx-mask';
import { NgxCaptchaModule } from 'ngx-captcha';
import { FooterComponent } from './shared/footer/footer.component';
import localePT from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
import { MAT_DATE_LOCALE } from '@angular/material/core';
registerLocaleData(localePT);

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    LoginComponent,
    MensagemConfirmacaoComponent,
    MensagemModalComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    EventsModule,
    AthletesModule,
    ManagerModule,
    PagamentosModule,
    HttpClientModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatIconModule,
    NgxCaptchaModule,
    NgxMaskModule.forRoot(),
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'pt-br' }],
  bootstrap: [AppComponent],
})
export class AppModule {}
