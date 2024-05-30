using TicketsRavelli.Controllers.RegistrosMedicos;
using TicketsRavelli.Core.Entities.Atletas;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface IRegistrosMedicosService {
        Task<RegistroMedico> ConsultaRegistroAtleta(string idAtleta);
        Task CadastrarRegistroMedico(RegistroMedicoInputModel registroMedicoInputModel);
        Task AtualizarRegistroMedico(RegistroMedico registroMedico, RegistroMedicoInputModel registroMedicoInputModel);
    }
}

