import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormasPagamentosComponent } from './formas-pagamentos/formas-pagamentos.component';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatRadioModule } from '@angular/material/radio';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@NgModule({
  declarations: [FormasPagamentosComponent],
  imports: [
    CommonModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatExpansionModule,
    MatRadioModule,
    MatInputModule,
    FormsModule,
    ClipboardModule,
    MatIconModule,
    MatTooltipModule,
  ],
})
export class PagamentosModule {}
