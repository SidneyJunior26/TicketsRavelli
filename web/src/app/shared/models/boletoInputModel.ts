export class BoletoInputModel {
  constructor(
    public nomeCliente: string,
    public email: string,
    public cpf: string,
    public telefone: string,
    public nomeProduto: string,
    public valor: string,
    public quantidade: number,
    public cupomDesconto: string
  ) {}
}
