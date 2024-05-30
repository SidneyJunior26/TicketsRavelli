import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { EventosService } from 'src/app/core/Eventos/events.service';
import { Pacotes } from 'src/app/shared/models/pacotes';

export interface DialogData {
  nomeEvento: string;
  eventoId: number;
}

@Component({
  selector: 'app-valores-evento',
  templateUrl: './valores-evento.component.html',
  styleUrls: ['./valores-evento.component.css'],
})
export class ValoresEventoComponent implements OnInit {
  constructor(
    private eventosService: EventosService,
    private dialogRef: MatDialogRef<ValoresEventoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  pacotes: Pacotes;
  carregandoPacotes: boolean = true;

  ngOnInit(): void {
    this.carregarValoresPacotes();
  }

  fecharDialog() {
    this.dialogRef.close();
  }

  private carregarValoresPacotes() {
    this.carregandoPacotes = true;
    this.eventosService
      .consultarPacotesPorEvento(this.data.eventoId)
      .subscribe((pacotes) => {
        this.pacotes = pacotes;
        this.carregandoPacotes = false;
      });
  }
}
