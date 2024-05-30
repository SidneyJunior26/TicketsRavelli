import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface DialogData {
  mensagem: string;
  icone: string;
}

@Component({
  selector: 'app-mensagem-modal',
  templateUrl: './mensagem-modal.component.html',
  styleUrls: ['./mensagem-modal.component.css'],
})
export class MensagemModalComponent implements OnInit {
  constructor(@Inject(MAT_DIALOG_DATA) public data: DialogData) {}

  ngOnInit(): void {}
}
