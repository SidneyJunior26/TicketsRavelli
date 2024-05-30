using Newtonsoft.Json;

namespace TicketsRavelli.Application.ViewModels.Relatorios {
    public class RelatorioInscricoesViewModel {

        public RelatorioInscricoesViewModel(bool? pago, decimal? valorPago, int? pacote,
            DateTime dataInscricao, DateTime? dataEfetivacao, string cpf, string nome, string rg,
            string federacao, DateTime? dataNascimento, string email, string celular, string cidade,
            string uF, string equipe, string dupla, string categoria, string camisa, string camisaCiclismo, string responsavel,
            string contatoEmergencia, string telEmergencia, string planoSaude, int pressaoAlta,
            int desmaio, int cadiaco, int diabetes, int asma, int alergia, string alergiaQual,
            int cirurgia, string? cirurgiaQual, int medicacao, string? medicacaoQual, string? medicacaoTempo,
            int malestar, string? malestarQual, int acompanhamento, string? acompanhamentoQual, string? outros)
        {
            if (pago != null & pago == true)
                Status = valorPago > 0 ? "Pago" : "Cortesia";
            else
                Status = "Não Pago";
            ValorPago = valorPago;
            Pacote = pacote;
            DataInscricao = dataInscricao.ToString("dd/MM/yyyy");
            if (dataEfetivacao.HasValue && dataEfetivacao != DateTime.MinValue)
                DataEfetivacao = dataEfetivacao.Value.ToString("dd/MM/yyyy");
            else
                DataEfetivacao = "";
            Cpf = cpf;
            Nome = nome;
            Rg = rg;
            Federacao = federacao;
            if (dataNascimento.HasValue && dataNascimento != DateTime.MinValue)
                DataNascimento = dataNascimento.Value.ToString("dd/MM/yyyy");
            else
                DataNascimento = "";
            Email = email;
            Celular = celular;
            Cidade = cidade;
            UF = uF;
            Categoria = categoria;
            Equipe = equipe;
            Dupla = dupla;
            Camisa = camisa;
            CamisaCiclismo = camisaCiclismo;
            Responsavel = responsavel;
            ContatoEmergencia = contatoEmergencia;
            TelEmergencia = telEmergencia;
            PlanoSaude = planoSaude;

            InfMedicas = $"{(pressaoAlta == 1 ? "Possuí Pressão Alta, " : "")}";
            InfMedicas += $"{(desmaio == 1 ? "Possuí Desmaios, " : "")}";
            InfMedicas += $"{(cadiaco == 1 ? "Possuí Ataque Cardiaco, " : "")}";
            InfMedicas += $"{(diabetes == 1 ? "Possuí Diabetes, " : "")}";
            InfMedicas += $"{(asma == 1 ? "Possuí Asma, " : "")}";
            InfMedicas += $"{(alergia == 1 ? $"Possuí Alergia a {alergiaQual}, " : "")}";
            InfMedicas += $"{(cirurgia == 1 ? $"Passou por Cirurgia: {cirurgiaQual}, " : "")}";
            InfMedicas += $"{(medicacao == 1 ? $"Usa medicação: {medicacaoQual} a {medicacaoTempo}" : "")}";
            InfMedicas += $"{(malestar == 1 ? $"Mal estar: {malestarQual}, " : "")}";
            InfMedicas += $"{(acompanhamento == 1 ? $"Acompanhamento: {acompanhamentoQual}, " : "")}";
            InfMedicas += $"{(outros != "" ? $"Outras Informações: {outros}" : "")}";
        }

        public string Status { get; set; }
        public decimal? ValorPago { get; set; }
        public int? Pacote { get; set; }
        [JsonProperty("Dt. Inscrição")]
        public string DataInscricao { get; set; }
        [JsonProperty("Dt. Efetivação")]
        public string DataEfetivacao { get; set; }
        [JsonProperty("CPf")]
        public string Cpf { get; set; }
        [JsonProperty("Nome")]
        public string Nome { get; set; }
        [JsonProperty("RG")]
        public string Rg { get; set; }
        [JsonProperty("Federação")]
        public string Federacao { get; set; }
        [JsonProperty("Dt. Nascimento")]
        public string DataNascimento { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        [JsonProperty("Cidade")]
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Categoria { get; set; }
        public string Equipe { get; set; }
        public string Dupla { get; set; }
        [JsonProperty("Camisa Normal")]
        public string Camisa { get; set; }
        [JsonProperty("Camisa Ciclismo")]
        public string CamisaCiclismo { get; set; }
        [JsonProperty("Responsável")]
        public string Responsavel { get; set; }
        [JsonProperty("Contato Emergência")]
        public string ContatoEmergencia { get; set; }
        [JsonProperty("Telefone Emergência")]
        public string TelEmergencia { get; set; }
        [JsonProperty("Plano Saúde")]
        public string PlanoSaude { get; set; }
        [JsonProperty("Informações Médicas")]
        public string InfMedicas { get; set; }
    }
}

