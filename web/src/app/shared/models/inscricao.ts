import { Atleta } from './Atleta/atleta';
import { Evento } from './evento';
import { SubCategoria } from './subCategoria';

export interface Subscription {
  [key: string]: any;
  id?: number;
  idEvento: number;
  evento?: Evento;
  cpfAtleta: string;
  atleta?: Atleta;
  idSubcategoria: number;
  subcategoria?: SubCategoria;
  equipe?: string;
  dupla?: string;
  quarteto: string;
  numeral?: string;
  dataInscricao?: Date;
  dataEfetivacao?: Date;
  pago?: boolean;
  valorPago?: number;
  pacote?: number;
  aceiteRegulamento: boolean;
  cancelado?: boolean;
  gnExpireAt?: Date;
  gnChargeTxId?: string;
  gnChargeId?: number;
  gnTotal?: number;
  gnLink?: string;
  gnBarCode?: string;
  gnStatus?: string;
  afiliadoId?: string;
}
