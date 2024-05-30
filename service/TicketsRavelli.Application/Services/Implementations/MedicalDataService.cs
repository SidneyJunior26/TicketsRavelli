using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Controllers.RegistrosMedicos;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations;

public class MedicalDataService : IMedicalDataService
{
    private readonly IMedicalDataRepository _medicalDataRepository;

    public MedicalDataService(IMedicalDataRepository medicalDataRepository)
    {
        _medicalDataRepository = medicalDataRepository;
    }

    public async Task UpdateAsync(RegistroMedico medicalData, MedicalDataInputModel medicalDataInputModel)
    {
        medicalData.Update(medicalDataInputModel.Plano,
                        medicalDataInputModel.PlanoEmpresa, medicalDataInputModel.PlanoTipo, medicalDataInputModel.PressaoAlta,
                        medicalDataInputModel.Desmaio, medicalDataInputModel.Cardiaco, medicalDataInputModel.Diabetes,
                        medicalDataInputModel.Asma, medicalDataInputModel.Alergia, medicalDataInputModel.AlergiaQual,
                        medicalDataInputModel.Cirurgia, medicalDataInputModel.CirurgiaQual, medicalDataInputModel.Medicacao,
                        medicalDataInputModel.MedicacaoQual, medicalDataInputModel.MedicacaoTempo, medicalDataInputModel.Malestar,
                        medicalDataInputModel.MalestarQual, medicalDataInputModel.Acompanhamento, medicalDataInputModel.AcompanhamentoQual,
                        medicalDataInputModel.Outros);

        await _medicalDataRepository.SaveChangesAsync();
    }

    public async Task CreateAsync(MedicalDataInputModel medicalDataInputModel)
    {
        var newMedicalData = new RegistroMedico(medicalDataInputModel.IdAtleta, medicalDataInputModel.Plano,
                                                medicalDataInputModel.PlanoEmpresa, medicalDataInputModel.PlanoTipo, medicalDataInputModel.PressaoAlta,
                                                medicalDataInputModel.Desmaio, medicalDataInputModel.Cardiaco, medicalDataInputModel.Diabetes,
                                                medicalDataInputModel.Asma, medicalDataInputModel.Alergia, medicalDataInputModel.AlergiaQual,
                                                medicalDataInputModel.Cirurgia, medicalDataInputModel.CirurgiaQual, medicalDataInputModel.Medicacao,
                                                medicalDataInputModel.MedicacaoQual, medicalDataInputModel.MedicacaoTempo, medicalDataInputModel.Malestar,
                                                medicalDataInputModel.MalestarQual, medicalDataInputModel.Acompanhamento, medicalDataInputModel.AcompanhamentoQual,
                                                medicalDataInputModel.Outros);

        await _medicalDataRepository.CreateAsync(newMedicalData);
        await _medicalDataRepository.SaveChangesAsync();
    }

    public async Task<RegistroMedico> GetByAthleteAsync(string idAthlete)
    {
        return await _medicalDataRepository.QueryByAthleteAsync(idAthlete);
    }
}

