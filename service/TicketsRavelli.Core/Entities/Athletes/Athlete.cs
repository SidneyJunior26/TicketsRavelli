using System.ComponentModel.DataAnnotations;
using Flunt.Notifications;

namespace TicketsRavelli.Core.Entities.Athletes;

public class Athlete : Notifiable<Notification>
{
    [Key]
    public string Id { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public DateTime Nascimento { get; set; }
    public string Sexo { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string? Rg { get; set; }
    public string? Responsavel { get; set; }
    public string Endereco { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string Complemento { get; set; } = null!;
    public string Cep { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Uf { get; set; } = null!;
    public string? Pais { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string Email { get; set; } = null!;
    public string? Profissao { get; set; }
    public string EmergenciaContato { get; set; } = null!;
    public string? EmergenciaFone { get; set; }
    public string? EmergenciaCelular { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public string? Camisa { get; set; }
    public string? CamisaCiclismo { get; set; }
    public string? MktLojaPreferida { get; set; }
    public string? MktBikePreferida { get; set; }
    public string? MktAro { get; set; }
    public string? MktCambio { get; set; }
    public string? MktFreio { get; set; }
    public string? MktSuspensao { get; set; }
    public string? MktMarcapneu { get; set; } = null!;
    public string? NktModelopneu { get; set; } = null!;
    public string? MktTenis { get; set; } = null!;
    public string Acesso { get; set; } = null!;
    public bool Ativo { get; set; }
    public string? Federacao { get; set; }
    public int Nivel { get; set; }
    public string? CodigoSenha { get; set; }
    public int PrimeiroAcesso { get; set; }
    public RegistroMedico RegistroMedico { get; set; }

    public Athlete()
    {

    }

    public Athlete(string nome, string nascimento, string sexo, string cpf, string rg, string responsavel,
        string endereco, string numero, string complemento, string cep, string cidade, string uf, string pais,
        string telefone, string celular, string email, string profissao, string emergenciaContato, string emergenciaFone,
        string emergenciaCelular, string camisa, string camisaCiclismo, string mktLojaPreferida, string mktBikePreferida,
        string mktAro, string mktCambio, string mktFreio, string mktSuspensao, string mktMarcaPneu, string mktModeloPneu,
        string mktTenis, string federacao, string acesso)
    {
        Id = Guid.NewGuid().ToString();

        Nome = nome;
        Nascimento = Convert.ToDateTime(nascimento);
        Sexo = sexo;
        Cpf = cpf;
        Rg = rg;
        Responsavel = responsavel;
        Endereco = endereco;
        Numero = numero;
        Complemento = complemento;
        Cep = cep;
        Cidade = cidade;
        Uf = uf;
        Pais = pais;
        Telefone = telefone;
        Celular = celular;
        Email = email;
        Profissao = profissao;
        EmergenciaContato = emergenciaContato;
        EmergenciaFone = emergenciaFone;
        EmergenciaCelular = emergenciaCelular;
        DataCadastro = DateTime.Now;
        DataAtualizacao = DateTime.Now;
        Camisa = camisa;
        CamisaCiclismo = camisaCiclismo;
        MktLojaPreferida = mktLojaPreferida;
        MktBikePreferida = mktBikePreferida;
        MktAro = mktAro;
        MktCambio = mktCambio;
        MktFreio = mktFreio;
        MktSuspensao = mktSuspensao;
        MktMarcapneu = mktMarcaPneu;
        NktModelopneu = mktModeloPneu;
        MktTenis = mktTenis;
        Federacao = federacao;
        Nivel = 1;
        Acesso = acesso;
        Ativo = true;
    }

    public void Update(string nome, DateTime nascimento, string sexo, string rg, string responsavel,
        string endereco, string numero, string complemento, string cep, string cidade, string uf, string pais,
        string telefone, string celular, string email, string profissao, string emergenciaContato, string emergenciaFone,
        string emergenciaCelular, string camisa, string camisaCiclismo, string mktLojaPreferida, string mktBikePreferida,
        string mktAro, string mktCambio, string mktFreio, string mktSuspensao, string mktMarcaPneu, string mktModeloPneu,
        string mktTenis, string federacao)
    {
        Nome = nome;
        Nascimento = nascimento;
        Sexo = sexo;
        Rg = rg;
        Responsavel = responsavel;
        Endereco = endereco;
        Numero = numero;
        Complemento = complemento;
        Cep = cep;
        Cidade = cidade;
        Uf = uf;
        Pais = pais;
        Telefone = telefone;
        Celular = celular;
        Email = email;
        Profissao = profissao;
        EmergenciaContato = emergenciaContato;
        EmergenciaFone = emergenciaFone;
        EmergenciaCelular = emergenciaCelular;
        DataAtualizacao = DateTime.Now;
        Camisa = camisa;
        CamisaCiclismo = camisaCiclismo;
        MktLojaPreferida = mktLojaPreferida;
        MktBikePreferida = mktBikePreferida;
        MktAro = mktAro;
        MktCambio = mktCambio;
        MktFreio = mktFreio;
        MktSuspensao = mktSuspensao;
        MktMarcapneu = mktMarcaPneu;
        NktModelopneu = mktModeloPneu;
        MktTenis = mktTenis;
        Federacao = federacao;
    }

    public void SalvarCodigoSenha(string codigo)
    {
        CodigoSenha = codigo;
    }

    public void UpdateShirt(string camisa, string camisaCiclismo)
    {
        Camisa = camisa;
        CamisaCiclismo = camisaCiclismo;
    }

    public void DefinirPrimeiraSenha(string acesso)
    {
        Acesso = acesso;
        CodigoSenha = string.Empty;
    }

    public void AtualizarSenha(string novaSenha)
    {
        Acesso = novaSenha;
        CodigoSenha = string.Empty;
    }
}
