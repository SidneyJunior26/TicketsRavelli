export interface SubCategoria {
  id?: number;
  idEvento: number;
  categoria: number;
  descSubcategoria: string | null;
  filtroSexo: number;
  filtroDupla: number;
  idadeDe: number;
  idadeAte: number;
  aviso?: string;
  ativo: boolean;
}
