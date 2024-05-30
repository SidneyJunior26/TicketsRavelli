export interface Afiliado {
  [key: string]: any;
  id?: number;
  cpf: string;
  nome: string;
  porcentagem: number;
  valorTotal?: number;
}
