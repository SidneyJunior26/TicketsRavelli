import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { Evento } from 'src/app/shared/models/evento';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { RelatoriosService } from 'src/app/core/Relatorios/relatorios.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSelect } from '@angular/material/select';

export interface DialogData {
  evento: Evento;
}

@Component({
  selector: 'app-relatorios',
  templateUrl: './relatorios.component.html',
  styleUrls: ['./relatorios.component.css'],
})
export class RelatoriosComponent implements OnInit {
  constructor(
    private relatoriosService: RelatoriosService,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  @ViewChild('selectRelatorio', { static: true }) selectRelatorio: MatSelect;

  gerandoRelatorio: boolean = false;

  ngOnInit(): void {
  }

  async exportarParaExcel(): Promise<void> {
    this.gerandoRelatorio = true;

    let fileName = '';
    var exportacaoDados: any[] = [];

    switch (this.selectRelatorio.value) {
      case '0':
        exportacaoDados = await this.gerarRelatorioInscritos();
        fileName = `Relatório de Inscritos - ${this.data.evento.nome}.xlsx`
        break;
      case '1':
        exportacaoDados = await this.gerarRelatorioInscritosPorCategoria();
        fileName = `Relatório de Inscritos (Categoria) - ${this.data.evento.nome}.xlsx`
        break;
      case '2':
        exportacaoDados = await this.gerarRelatorioTotalInscritosCategoria();
        fileName = `Total de Inscritos - Categoria - ${this.data.evento.nome}.xlsx`
        break;
      case '3':
        exportacaoDados = await this.gerarRelatorioTotalInscritosEfetivados();
        fileName = `Total de Inscritos - Efetivados - ${this.data.evento.nome}.xlsx`
        break;
      case '4':
        exportacaoDados = await this.gerarRelatorioTotalCamisasCategoria();
        fileName = `Total de camisas por tamanho - ${this.data.evento.nome}.xlsx`
        break;
    }

    if (exportacaoDados.length == 0) {
      return;
    }
    // Criando uma planilha do Excel
    const workbook: XLSX.WorkBook = XLSX.utils.book_new();
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportacaoDados);

    // Adicionando a planilha ao livro de trabalho
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Dados');

    // Gerando o arquivo Excel a partir do livro de trabalho
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    // Criando um blob a partir do buffer do Excel
    const blob: Blob = new Blob([excelBuffer], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    });

    // Salvando o arquivo Excel no navegador para download
    //const fileName: string = `Inscricoes - ${this.data.evento.nome}.xlsx`;
    saveAs(blob, fileName);
  }

  private async gerarRelatorioInscritos(): Promise<any[]> {
    return new Promise<any[]>((resolve, reject) => {
      this.relatoriosService
        .consultarRelatorioInscricoesEvento(this.data.evento.id!)
        .subscribe(
          (dados: any[]) => {
            console.log(dados);
            const exportacaoDados = dados.map((item) => (
              {
              Status: item.status,
              'Valor Pago': item.valorPago,
              Pacote: item.pacote,
              'Dt. Inscrição': item.dataInscricao,
              'Dt. de Efetivação': item.dataEfetivacao,
              CPF: item.cpf,
              Nome: item.nome,
              RG: item.rg,
              'Federação:': item.federacao,
              'Dt. Nascimento': item.dataNascimento,
              Email: item.email,
              Celular: item.celular,
              Cidade: item.cidade,
              UF: item.uf,
              Equipe: item.equipe,
              Dupla: item.dupla,
              Categoria: item.categoria,
              'Camisa Normal': item.camisa,
              'Camisa Ciclismo': item.camisaCiclismo,
              'Responsável': item.responsavel,
              'Contato Emergência': item.contatoEmergencia,
              'Tel. Emerg&encia': item.telEmergencia,
              'Plano Saúde': item.planoSaude,
              'Inf. Médicas': item.infMedicas
            }));
            resolve(exportacaoDados);

            this.gerandoRelatorio = false;
          },
          (error) => {
            reject(error);

            this.gerandoRelatorio = false;
          }
        );
    });
  }

  private async gerarRelatorioInscritosPorCategoria(): Promise<any[]> {
    return new Promise<any[]>((resolve, reject) => {
      this.relatoriosService
        .consultarRelatorioInscricoesEventoPorCategoria(this.data.evento.id!)
        .subscribe(
          (dados: any[]) => {
            const exportacaoDados = dados.map((item) => ({
              'Data de Efetivação': item.dataEfetivacao,
              CPF: item.cpf,
              Nome: item.nomeAtleta,
              Cidade: item.cidade,
              UF: item.uf,
              Email: item.email,
              Categoria: item.categoria,
              Equipe: item.equipe,
              Dupla: item.dupla,
              Camisa: item.camisa,
              'Camisa Ciclismo': item.camisaCiclismo,
              Pago: item.pago,
            }));
            resolve(exportacaoDados);

            this.gerandoRelatorio = false;
          },
          (error) => {
            reject(error);

            this.gerandoRelatorio = false;
          }
        );
    });
  }

  private async gerarRelatorioTotalInscritosCategoria(): Promise<any[]> {
    return new Promise<any[]>((resolve, reject) => {
      this.relatoriosService
        .consultarRelatorioTotalInscritosCategoria(this.data.evento.id!)
        .subscribe(
          (dados: any[]) => {
            const exportacaoDados = dados.map((item) => ({
              Categoria: item.subCategoria,
              'Total Inscritos': item.totalInscritos,
            }));
            resolve(exportacaoDados);

            this.gerandoRelatorio = false;
          },
          (error) => {
            reject(error);

            this.gerandoRelatorio = false;
          }
        );
    });
  }

  private async gerarRelatorioTotalInscritosEfetivados(): Promise<any[]> {
    return new Promise<any[]>((resolve, reject) => {
      this.relatoriosService
        .consultarRelatorioTotalInscritosEfetivados(this.data.evento.id!)
        .subscribe(
          (dados: any[]) => {
            const exportacaoDados = dados.map((item) => ({
              Categoria: item.subCategoria,
              'Total Inscritos': item.totalInscritos,
            }));
            resolve(exportacaoDados);

            this.gerandoRelatorio = false;
          },
          (error) => {
            reject(error);

            this.gerandoRelatorio = false;
          }
        );
    });
  }

  private async gerarRelatorioTotalCamisasCategoria(): Promise<any[]> {
    return new Promise<any[]>((resolve, reject) => {
      this.relatoriosService
        .consultarRelatorioTotalCamisasCategoria(this.data.evento.id!)
        .subscribe(
          (dados: any[]) => {
            const exportacaoDados = dados.map((item) => ({
              Camisa: item.camisa,
              'Total Inscritos': item.totalInscritos,
            }));
            resolve(exportacaoDados);

            this.gerandoRelatorio = false;
          },
          (error) => {
            reject(error);

            this.gerandoRelatorio = false;
          }
        );
    });
  }
}
