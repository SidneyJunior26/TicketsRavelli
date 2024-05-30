import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface DialogData {
  mensagem: string;
}

@Component({
  selector: 'app-mensagem-confirmacao',
  templateUrl: './mensagem-confirmacao.component.html',
  styleUrls: ['./mensagem-confirmacao.component.css'],
})
export class MensagemConfirmacaoComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<MensagemConfirmacaoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  ngOnInit(): void {}

  cancelar() {
    this.dialogRef.close(false);
  }

  confirmar(): void {
    this.dialogRef.close(true);
  }
}
