using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Core.Entities.Afiliados;

public class Affiliate {
    public string Id { get; set; }
    public string Cpf { get; set; }
    public string Nome { get; set; }
    public int Porcentagem { get; set; }
    public string Link { get; set; }
    public List<Subscription>? Inscricoes { get; set; }

    public Affiliate() {

    }

    public Affiliate(string cpf, string nome, int porcentagem) {
        Id = Guid.NewGuid().ToString();

        Cpf = cpf;
        Nome = nome;
        Porcentagem = porcentagem;
        Link = "https://ticketsravelli.com.br/" + Id;
    }

    public void Update(string cpf, string nome, int porcentagem) {
        Cpf = cpf;
        Nome = nome;
        Porcentagem = porcentagem;
    }
}

