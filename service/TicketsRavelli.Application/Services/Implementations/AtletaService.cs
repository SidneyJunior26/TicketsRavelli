using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using TicketsRavelli.Application.InputModels.Atletas;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Atletas;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;
using TicketsRavelli.Infrastructure.Security.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class AtletaService : IAthleteService {
        private readonly IAtletaRepository _atletaRepository;
        private readonly ICriptografiaService _criptografiaService;
        private readonly string _connectionString;

        public AtletaService(IAtletaRepository atletaRepository, ICriptografiaService criptografiaService,
            IConfiguration configuration) {
            _atletaRepository = atletaRepository;
            _criptografiaService = criptografiaService;

            _connectionString = configuration.GetConnectionString("MySql");
        }

        public async Task AtualizarCpfAtleta(Atleta atleta, string cpf) {
            await using (var sqlConnection = new MySqlConnection(_connectionString)) {
                sqlConnection.Open();

                var script = $"UPDATE ATLETA SET CPF = {cpf} WHERE CPF = {atleta.Cpf}";

                await sqlConnection.ExecuteAsync(script);
            }
        }

        public async Task AtualizarAtleta(Atleta atleta, UpdateAthleteInputModel atletaInputModel) {
            atleta.atualizarDadosAtleta(atletaInputModel.Nome, Convert.ToDateTime(atletaInputModel.Nascimento), atletaInputModel.Sexo,
                atletaInputModel.Rg, atletaInputModel.Responsavel, atletaInputModel.Endereco,
                atletaInputModel.Numero, atletaInputModel.Complemento, atletaInputModel.Cep, atletaInputModel.Cidade,
                atletaInputModel.Uf, atletaInputModel.Pais, atletaInputModel.Telefone, atletaInputModel.Celular,
                atletaInputModel.Email, atletaInputModel.Profissao, atletaInputModel.EmergenciaContato,
                atletaInputModel.EmergenciaFone, atletaInputModel.EmergenciaCelular, atletaInputModel.Camisa,
                atletaInputModel.CamisaCiclismo, atletaInputModel.MktLojaPreferida, atletaInputModel.MktBikePreferida,
                atletaInputModel.MktAro, atletaInputModel.MktCambio, atletaInputModel.MktFreio, atletaInputModel.MktSuspensao,
                atletaInputModel.MktMarcaPneu, atletaInputModel.MktModeloPneu, atletaInputModel.MktTenis,
                atletaInputModel.Federacao);

            await _atletaRepository.SalvarAlteracoesAsync();
        }

        public async Task AtualizarCamisasAtletas(Atleta atleta, AtualizaCamisaInputModel atualizaCamisaInputModel) {
            atleta.AtualizarCamisas(atualizaCamisaInputModel.camisa, atualizaCamisaInputModel.camisaCiclismo);

            await _atletaRepository.SalvarAlteracoesAsync();
        }

        public async Task<Atleta> CadastrarAtleta<T>(T inputModel) {
            Atleta novoAtleta = new Atleta();

            if (inputModel is NewAthleteInputModel atletaInputModel)            {
                novoAtleta = new Atleta(atletaInputModel.Nome, atletaInputModel.Nascimento, atletaInputModel.Sexo,
                    atletaInputModel.Cpf, atletaInputModel.Rg, atletaInputModel.Responsavel, atletaInputModel.Endereco,
                    atletaInputModel.Numero, atletaInputModel.Complemento, atletaInputModel.Cep, atletaInputModel.Cidade,
                    atletaInputModel.Uf, atletaInputModel.Pais, atletaInputModel.Telefone, atletaInputModel.Celular,
                    atletaInputModel.Email, atletaInputModel.Profissao, atletaInputModel.EmergenciaContato,
                    atletaInputModel.EmergenciaFone, atletaInputModel.EmergenciaCelular, atletaInputModel.Camisa,
                    atletaInputModel.CamisaCiclismo, atletaInputModel.MktLojaPreferida, atletaInputModel.MktBikePreferida,
                    atletaInputModel.MktAro, atletaInputModel.MktCambio, atletaInputModel.MktFreio, atletaInputModel.MktSuspensao,
                    atletaInputModel.MktMarcaPneu, atletaInputModel.MktModeloPneu, atletaInputModel.MktTenis,
                    atletaInputModel.Federacao, _criptografiaService.Criptografar(atletaInputModel.Acesso));
            } else if (inputModel is NovoAtletaAdmInputModel atletaAdmInputModel) {
                novoAtleta = new Atleta(atletaAdmInputModel.Nome, atletaAdmInputModel.Nascimento, atletaAdmInputModel.Sexo,
                    atletaAdmInputModel.Cpf, atletaAdmInputModel.Rg, atletaAdmInputModel.Responsavel, atletaAdmInputModel.Endereco,
                    atletaAdmInputModel.Numero, atletaAdmInputModel.Complemento, atletaAdmInputModel.Cep, atletaAdmInputModel.Cidade,
                    atletaAdmInputModel.Uf, atletaAdmInputModel.Pais, atletaAdmInputModel.Telefone, atletaAdmInputModel.Celular,
                    atletaAdmInputModel.Email, atletaAdmInputModel.Profissao, atletaAdmInputModel.EmergenciaContato,
                    atletaAdmInputModel.EmergenciaFone, atletaAdmInputModel.EmergenciaCelular, atletaAdmInputModel.Camisa,
                    atletaAdmInputModel.CamisaCiclismo, atletaAdmInputModel.MktLojaPreferida, atletaAdmInputModel.MktBikePreferida,
                    atletaAdmInputModel.MktAro, atletaAdmInputModel.MktCambio, atletaAdmInputModel.MktFreio, atletaAdmInputModel.MktSuspensao,
                    atletaAdmInputModel.MktMarcaPneu, atletaAdmInputModel.MktModeloPneu, atletaAdmInputModel.MktTenis,
                    atletaAdmInputModel.Federacao, _criptografiaService.Criptografar(atletaAdmInputModel.Acesso));
            }

            try {
                await _atletaRepository.CadastrarAtleta(novoAtleta);
                await _atletaRepository.SalvarAlteracoesAsync();
            } catch (Exception ex) {
                return novoAtleta;
            }

            return novoAtleta;
        }

        public async Task<Atleta> GetAthleteByCpfAsync(string cpf) {
            return await _atletaRepository.ConsultarAtletaPorCpf(cpf);
        }

        public async Task<Atleta> ConsultarAtletaPorCpfOuEmail(string cpfEmail) {
            return await _atletaRepository.ConsultarAtletaPorCpf(cpfEmail);
        }

        public async Task<Atleta> ConsultarAtletaPorEmail(string email) {
            return await _atletaRepository.ConsultarAtletaPorEmail(email);
        }

        public async Task<Atleta> ConsultarAtletaPorId(string id) {
            return await _atletaRepository.ConsultarAtletaPorId(id);
        }

        public async Task<List<Atleta>> GetAllAthletesAsync() {
            return await _atletaRepository.ConsultarTodosAtletas();
        }

        public async Task DeletarAtleta(Atleta atleta) {
            _atletaRepository.DeletarAtleta(atleta);
            await _atletaRepository.SalvarAlteracoesAsync();
        }

        public async Task<bool> CheckAthleteExistsAsync(string cpf) {
            var atletaExiste = await _atletaRepository.VerificaAtletaExisteCpf(cpf);

            return atletaExiste;
        }

        public async Task<bool> CheckAthleteRegisteredByEmailAsync(string email) {
            var atletaExiste = await _atletaRepository.VerificaAtletaExisteEmail(email);

            return atletaExiste;
        }

        public async Task<bool> VerificaAtletaExisteId(string id) {
            var atletaExiste = await _atletaRepository.VerificaAtletaExisteId(id);

            return atletaExiste;
        }

        public async Task<bool> CheckAthleteRegisteredByRGAsync(string rg) {
            var atleta = await _atletaRepository.VerificaAtletaExisteRG(rg);

            if (atleta == null)
                return false;

            return true;
        }
    }
}