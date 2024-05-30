using TicketsRavelli.Application.InputModels.Atletas;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;
using TicketsRavelli.Infrastructure.Security.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class AthleteService : IAthleteService
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly ICryptographyService _cryptographyService;

        public AthleteService(IAthleteRepository athleteRepository, ICryptographyService cryptographyService)
        {
            _athleteRepository = athleteRepository;
            _cryptographyService = cryptographyService;
        }

        public async Task UpdateCpfAsync(Athlete athlete, string cpf)
        {
            await _athleteRepository.UpdateCpfAsync(athlete, cpf);
        }

        public async Task UpdateAsync(Athlete athlete, UpdateAthleteInputModel athleteInputModel)
        {
            athlete.Update(athleteInputModel.Nome, Convert.ToDateTime(athleteInputModel.Nascimento), athleteInputModel.Sexo,
                athleteInputModel.Rg, athleteInputModel.Responsavel, athleteInputModel.Endereco,
                athleteInputModel.Numero, athleteInputModel.Complemento, athleteInputModel.Cep, athleteInputModel.Cidade,
                athleteInputModel.Uf, athleteInputModel.Pais, athleteInputModel.Telefone, athleteInputModel.Celular,
                athleteInputModel.Email, athleteInputModel.Profissao, athleteInputModel.EmergenciaContato,
                athleteInputModel.EmergenciaFone, athleteInputModel.EmergenciaCelular, athleteInputModel.Camisa,
                athleteInputModel.CamisaCiclismo, athleteInputModel.MktLojaPreferida, athleteInputModel.MktBikePreferida,
                athleteInputModel.MktAro, athleteInputModel.MktCambio, athleteInputModel.MktFreio, athleteInputModel.MktSuspensao,
                athleteInputModel.MktMarcaPneu, athleteInputModel.MktModeloPneu, athleteInputModel.MktTenis,
                athleteInputModel.Federacao);

            await _athleteRepository.SaveChangesAsync();
        }

        public async Task UpdateShirtAsync(Athlete athlete, UpdateShirtInputModel updateShirtInputModel)
        {
            athlete.UpdateShirt(updateShirtInputModel.camisa, updateShirtInputModel.camisaCiclismo);

            await _athleteRepository.SaveChangesAsync();
        }

        public async Task<Athlete> CreateAsync<T>(T inputModel)
        {
            Athlete newAthlete = new Athlete();

            if (inputModel is NewAthleteInputModel athleteInputModel)
            {
                newAthlete = new Athlete(athleteInputModel.Nome, athleteInputModel.Nascimento, athleteInputModel.Sexo,
                    athleteInputModel.Cpf, athleteInputModel.Rg, athleteInputModel.Responsavel, athleteInputModel.Endereco,
                    athleteInputModel.Numero, athleteInputModel.Complemento, athleteInputModel.Cep, athleteInputModel.Cidade,
                    athleteInputModel.Uf, athleteInputModel.Pais, athleteInputModel.Telefone, athleteInputModel.Celular,
                    athleteInputModel.Email, athleteInputModel.Profissao, athleteInputModel.EmergenciaContato,
                    athleteInputModel.EmergenciaFone, athleteInputModel.EmergenciaCelular, athleteInputModel.Camisa,
                    athleteInputModel.CamisaCiclismo, athleteInputModel.MktLojaPreferida, athleteInputModel.MktBikePreferida,
                    athleteInputModel.MktAro, athleteInputModel.MktCambio, athleteInputModel.MktFreio, athleteInputModel.MktSuspensao,
                    athleteInputModel.MktMarcaPneu, athleteInputModel.MktModeloPneu, athleteInputModel.MktTenis,
                    athleteInputModel.Federacao, _cryptographyService.Criptografar(athleteInputModel.Acesso));
            }
            else if (inputModel is NewAthleteInputModel athleteAdmInputModel)
            {
                newAthlete = new Athlete(athleteAdmInputModel.Nome, athleteAdmInputModel.Nascimento, athleteAdmInputModel.Sexo,
                    athleteAdmInputModel.Cpf, athleteAdmInputModel.Rg, athleteAdmInputModel.Responsavel, athleteAdmInputModel.Endereco,
                    athleteAdmInputModel.Numero, athleteAdmInputModel.Complemento, athleteAdmInputModel.Cep, athleteAdmInputModel.Cidade,
                    athleteAdmInputModel.Uf, athleteAdmInputModel.Pais, athleteAdmInputModel.Telefone, athleteAdmInputModel.Celular,
                    athleteAdmInputModel.Email, athleteAdmInputModel.Profissao, athleteAdmInputModel.EmergenciaContato,
                    athleteAdmInputModel.EmergenciaFone, athleteAdmInputModel.EmergenciaCelular, athleteAdmInputModel.Camisa,
                    athleteAdmInputModel.CamisaCiclismo, athleteAdmInputModel.MktLojaPreferida, athleteAdmInputModel.MktBikePreferida,
                    athleteAdmInputModel.MktAro, athleteAdmInputModel.MktCambio, athleteAdmInputModel.MktFreio, athleteAdmInputModel.MktSuspensao,
                    athleteAdmInputModel.MktMarcaPneu, athleteAdmInputModel.MktModeloPneu, athleteAdmInputModel.MktTenis,
                    athleteAdmInputModel.Federacao, _cryptographyService.Criptografar(athleteAdmInputModel.Acesso));
            }

            try
            {
                await _athleteRepository.CreateAsync(newAthlete);
                await _athleteRepository.SaveChangesAsync();
            }
            catch
            {
                return newAthlete;
            }

            return newAthlete;
        }

        public async Task<Athlete> GetByCpfAsync(string cpf)
        {
            return await _athleteRepository.QueryByCPF(cpf);
        }

        public async Task<Athlete> GetByCpfOrEmailAsync(string cpfEmail)
        {
            return await _athleteRepository.QueryAthleteByEmailOrCPF(cpfEmail);
        }

        public async Task<Athlete> GetByEmailAsync(string email)
        {
            return await _athleteRepository.QueryByEmail(email);
        }

        public async Task<Athlete> GetByIdAsync(string id)
        {
            return await _athleteRepository.QueryById(id);
        }

        public async Task<List<Athlete>> GetAllAsync()
        {
            return await _athleteRepository.QueryAllAsync();
        }

        public async Task DeleteAsync(Athlete athlete)
        {
            _athleteRepository.Delete(athlete);

            await _athleteRepository.SaveChangesAsync();
        }

        public async Task<bool> CheckAthleteExistsByCpfAsync(string cpf)
        {
            var athleteExists = await _athleteRepository.AthleteExistsByCPF(cpf);

            return athleteExists;
        }

        public async Task<bool> CheckAthleteRegisteredByEmailAsync(string email)
        {
            var athleteExists = await _athleteRepository.AthleteExistsByEmail(email);

            return athleteExists;
        }

        public async Task<bool> CheckAthleteExistsByIdAsync(string id)
        {
            var athleteExists = await _athleteRepository.AthleteExistsById(id);

            return athleteExists;
        }

        public async Task<bool> CheckAthleteRegisteredByRGAsync(string rg)
        {
            var athlete = await _athleteRepository.QueryAthleteByRG(rg);

            if (athlete == null)
                return false;

            return true;
        }
    }
}