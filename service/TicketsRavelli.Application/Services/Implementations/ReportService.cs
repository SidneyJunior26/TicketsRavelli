using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Application.ViewModels.Relatorios;
using TicketsRavelli.Infra.Data;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RelatorioInscricoesViewModel>> GetSubscriptionsCategoriesReportAsync(int idEvento)
        {
            var inscricoes = _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta).ThenInclude(a => a.RegistroMedico)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvento)
                .OrderBy(i => i.DataEfetivacao);

            var relatorioInscricoesCategoria = await inscricoes
                .AsNoTracking()
                .Select(i => new RelatorioInscricoesViewModel(
                    i.Pago,
                    i.ValorPago,
                    i.Pacote,
                    i.DataInscricao,
                    i.DataEfetivacao,
                    i.CpfAtleta,
                    i.Atleta.Nome,
                    i.Atleta.Rg,
                    i.Atleta.Federacao,
                    i.Atleta.Nascimento,
                    i.Atleta.Email,
                    i.Atleta.Celular,
                    i.Atleta.Cidade,
                    i.Atleta.Uf,
                    i.Equipe,
                    i.Dupla,
                    i.Subcategoria.DescSubcategoria,
                    i.Atleta.Camisa,
                    i.Atleta.CamisaCiclismo,
                    i.Atleta.Responsavel,
                    i.Atleta.EmergenciaContato,
                    i.Atleta.EmergenciaCelular,
                    i.Atleta.RegistroMedico.PlanoEmpresa,
                    i.Atleta.RegistroMedico.Pressaoalta,
                    i.Atleta.RegistroMedico.Desmaio,
                    i.Atleta.RegistroMedico.Cardiaco,
                    i.Atleta.RegistroMedico.Diabetes,
                    i.Atleta.RegistroMedico.Asma,
                    i.Atleta.RegistroMedico.Alergia,
                    i.Atleta.RegistroMedico.AlergiaQual,
                    i.Atleta.RegistroMedico.Cirurgia,
                    i.Atleta.RegistroMedico.CirurgiaQual,
                    i.Atleta.RegistroMedico.Medicacao,
                    i.Atleta.RegistroMedico.MedicacaoQual,
                    i.Atleta.RegistroMedico.MedicacaoTempo,
                    i.Atleta.RegistroMedico.Malestar,
                    i.Atleta.RegistroMedico.MalestarQual,
                    i.Atleta.RegistroMedico.Acompanhamento,
                    i.Atleta.RegistroMedico.AcompanhamentoQual,
                    i.Atleta.RegistroMedico.Outros
                    )).ToListAsync();

            return relatorioInscricoesCategoria;
        }

        public async Task<List<RelatorioTotalInscritosCategoriaInpurModel>> GetTotalSubscriptionsByCategoriesReportAsync(int idEvento)
        {
            var inscricoes = _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvento)
                .GroupBy(i => i.Subcategoria.DescSubcategoria)
                .Select(group => new
                {
                    DescricaoSubcategoria = group.Key,
                    Quantidade = group.Count()
                })
                .OrderBy(result => result.DescricaoSubcategoria);

            var relatorio = await inscricoes.AsNoTracking().Select(i => new RelatorioTotalInscritosCategoriaInpurModel(
                i.DescricaoSubcategoria,
                i.Quantidade)).ToListAsync();

            return relatorio;
        }

        public async Task<List<RelatorioInscricoesViewModel>> GetSubscriptionsReportAsync(int idEvento)
        {
            var inscricoes = _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta).ThenInclude(a => a.RegistroMedico)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvento)
                .OrderBy(i => i.DataEfetivacao);

            try
            {
                var teste = await inscricoes
                .AsNoTracking()
                .Select(i => new RelatorioInscricoesViewModel(
                    i.Pago,
                    i.ValorPago,
                    i.Pacote,
                    i.DataInscricao,
                    i.DataEfetivacao,
                    i.CpfAtleta,
                    i.Atleta.Nome,
                    i.Atleta.Rg,
                    i.Atleta.Federacao,
                    i.Atleta.Nascimento,
                    i.Atleta.Email,
                    i.Atleta.Celular,
                    i.Atleta.Cidade,
                    i.Atleta.Uf,
                    i.Equipe,
                    i.Dupla,
                    i.Subcategoria.DescSubcategoria,
                    i.Atleta.Camisa,
                    i.Atleta.CamisaCiclismo,
                    i.Atleta.Responsavel,
                    i.Atleta.EmergenciaContato,
                    i.Atleta.EmergenciaCelular,
                    "",
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    "",
                    0,
                    "",
                    0,
                    "",
                    "",
                    0,
                    "",
                    0,
                    "",
                    ""
                    )).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            var relatorioInscricoes = await inscricoes
                .AsNoTracking()
                .Select(i => new RelatorioInscricoesViewModel(
                    i.Pago,
                    i.ValorPago,
                    i.Pacote,
                    i.DataInscricao,
                    i.DataEfetivacao,
                    i.CpfAtleta,
                    i.Atleta.Nome,
                    i.Atleta.Rg,
                    i.Atleta.Federacao,
                    i.Atleta.Nascimento,
                    i.Atleta.Email,
                    i.Atleta.Celular,
                    i.Atleta.Cidade,
                    i.Atleta.Uf,
                    i.Equipe,
                    i.Dupla,
                    i.Subcategoria.DescSubcategoria,
                    i.Atleta.Camisa,
                    i.Atleta.CamisaCiclismo,
                    i.Atleta.Responsavel,
                    i.Atleta.EmergenciaContato,
                    i.Atleta.EmergenciaCelular,
                    i.Atleta.RegistroMedico.PlanoEmpresa,
                    i.Atleta.RegistroMedico.Pressaoalta,
                    i.Atleta.RegistroMedico.Desmaio,
                    i.Atleta.RegistroMedico.Cardiaco,
                    i.Atleta.RegistroMedico.Diabetes,
                    i.Atleta.RegistroMedico.Asma,
                    i.Atleta.RegistroMedico.Alergia,
                    i.Atleta.RegistroMedico.AlergiaQual,
                    i.Atleta.RegistroMedico.Cirurgia,
                    i.Atleta.RegistroMedico.CirurgiaQual,
                    i.Atleta.RegistroMedico.Medicacao,
                    i.Atleta.RegistroMedico.MedicacaoQual,
                    i.Atleta.RegistroMedico.MedicacaoTempo,
                    i.Atleta.RegistroMedico.Malestar,
                    i.Atleta.RegistroMedico.MalestarQual,
                    i.Atleta.RegistroMedico.Acompanhamento,
                    i.Atleta.RegistroMedico.AcompanhamentoQual,
                    i.Atleta.RegistroMedico.Outros
                    )).ToListAsync();

            return relatorioInscricoes;
        }

        public async Task<List<RelatorioTotalInscritosCategoriaInpurModel>> GetTotalEffectiveSubscriptionsAsync(int idEvento)
        {
            var inscricoes = _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvento && i.DataEfetivacao != null)
                .GroupBy(i => i.Subcategoria.DescSubcategoria)
                .Select(group => new
                {
                    DescricaoSubcategoria = group.Key,
                    Quantidade = group.Count()
                })
                .OrderBy(result => result.DescricaoSubcategoria);

            var relatorio = await inscricoes
                .AsNoTracking()
                .Select(i => new RelatorioTotalInscritosCategoriaInpurModel(
                i.DescricaoSubcategoria,
                i.Quantidade)).ToListAsync();

            return relatorio;
        }

        public async Task<List<RelatorioTotalCamisasCategoriaInputModel>> GetTotalShirtsCategoriesAsync(int idEvento)
        {
            var camisasNormais = await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvento && i.Atleta.Camisa != null && i.Pacote == 1)
                .GroupBy(i => i.Atleta.Camisa)
                .Select(group => new
                {
                    Camisa = "Camisa Normal: " + group.Key,
                    Quantidade = group.Count()
                })
                .OrderBy(result => result.Quantidade)
                .ToListAsync();

            var camisasCiclismo = await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvento && i.Atleta.CamisaCiclismo != null && (i.Pacote == 2 || i.Pacote == 3))
                .GroupBy(i => i.Atleta.CamisaCiclismo)
                .Select(group => new
                {
                    CamisaCiclismo = "Camisa Ciclismo: " + group.Key,
                    Quantidade = group.Count()
                })
                .OrderBy(result => result.Quantidade)
                .ToListAsync();

            var relatorio = new List<RelatorioTotalCamisasCategoriaInputModel>();

            foreach (var item in camisasNormais)
            {
                relatorio.Add(new RelatorioTotalCamisasCategoriaInputModel(item.Camisa, "", item.Quantidade));
            }

            foreach (var item in camisasCiclismo)
            {
                relatorio.Add(new RelatorioTotalCamisasCategoriaInputModel(item.CamisaCiclismo, "", item.Quantidade));
            }

            return relatorio;
        }

    }
}