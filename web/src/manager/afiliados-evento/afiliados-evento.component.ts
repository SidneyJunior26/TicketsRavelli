import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { AfiliadoService } from 'src/app/core/Afiliado/afiliado.service';
import { Afiliado } from 'src/app/shared/models/afiliado';
import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { Evento } from 'src/app/shared/models/evento';

export interface DialogData {
  evento: Evento;
}

@Component({
  selector: 'app-afiliados-evento',
  templateUrl: './afiliados-evento.component.html',
  styleUrls: ['./afiliados-evento.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition(
        'expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
  standalone: true,
  imports: [
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    CurrencyPipe,
    CommonModule,
  ],
})
export class AfiliadosEventoComponent implements OnInit {
  constructor(
    private afiliadoService: AfiliadoService,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  afiliados: Afiliado[] = [];
  dsAfiliados: MatTableDataSource<Afiliado>;
  expandedElement: Afiliado | null;

  displayedColumnsAfiliados: string[] = [
    'afiliado',
    'qtde',
    'porcentagem',
    'valorTotal',
    'valorAfiliado',
  ];
  columnsToDisplayWithExpand = [...this.displayedColumnsAfiliados, 'expand'];

  ngOnInit(): void {
    this.consultarAfiliados();
  }

  private consultarAfiliados() {
    this.afiliadoService
      .consultarAfiliadosEvento(this.data.evento.id!)
      .subscribe((afiliados) => {
        console.log(afiliados);
        afiliados.forEach(
          (afiliado: { valorTotal: any; inscricoes: any[] }) => {
            afiliado.valorTotal = afiliado.inscricoes.reduce(
              (total, inscricao) => total + (inscricao.valorPago || 0),
              0
            );
          }
        );

        this.afiliados = afiliados;
        this.dsAfiliados = afiliados;

        this.dsAfiliados = new MatTableDataSource(this.afiliados);
      });
  }
}
