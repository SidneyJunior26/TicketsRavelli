export interface NovaSenha {
  cpf: string;
  senhaAtual: string;
  novaSenha: string;
}

export interface NovaSenhaCodigo {
  id: string;
  codigo: string;
  novaSenha: string;
}

export interface EsqueciSenha {
  cpfEmail: string;
  codigo: string;
  novaSenha: string;
}

export interface ManagerNovaSenha {
  id: string;
  novaSenha: string;
}
