export interface Categoria {
  [key: string]: any;
  id?: number;
  idEvento: number;
  categoria: number;
  descSubcategoria: string;
  filtroSexo: number;
  filtroDupla: number;
  idadeDe: number;
  idadeAte: number;
  aviso: string;
  ativo: boolean;
  percurso?: string;
}
