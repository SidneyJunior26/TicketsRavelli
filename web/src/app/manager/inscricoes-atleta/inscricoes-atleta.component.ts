import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { SubscriptionsService } from 'src/app/core/Inscricoes/inscricoes.service';
import { InscricoesAtleta } from 'src/app/shared/models/inscricoesAtleta';

export interface DialogData {
  cpfAtleta: string;
}

@Component({
  selector: 'app-inscricoes-atleta',
  templateUrl: './inscricoes-atleta.component.html',
  styleUrls: ['./inscricoes-atleta.component.css'],
})
export class InscricoesAtletaComponent implements OnInit {
  constructor(
    private inscricoesService: SubscriptionsService,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  carregandoInscricoes: boolean = false;

  inscricoesAtleta: InscricoesAtleta[] = [];

  dsInscricoes: MatTableDataSource<InscricoesAtleta>;
  displayedColumns = ['evento', 'categoria', 'pacote', 'pago'];

  ngOnInit(): void {
    this.carregarInscricoes();
  }

  private carregarInscricoes() {
    this.carregandoInscricoes = true;

    this.inscricoesService
      .getAthleteSubscriptions(this.data.cpfAtleta)
      .subscribe(
        (inscricoes) => {
          this.inscricoesAtleta = inscricoes;

          this.dsInscricoes = new MatTableDataSource(this.inscricoesAtleta);

          this.carregandoInscricoes = false;
        },
        (error) => {
          this.carregandoInscricoes = false;
        }
      );
  }
}
