export interface InscricoesAtleta {
  id: number;
  idEvento: number;
  nomeEvento: string;
  nomeCategoria: string;
  dataEvento: Date;
  pago: boolean;
  inscricoesAbertas: boolean;
  linkBoleto: string;
  permiteNovoBoleto: boolean;
}
