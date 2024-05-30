using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Controllers.RegistrosMedicos;
using TicketsRavelli.Core.Entities.Atletas;
using TicketsRavelli.Infra.Data;

namespace TicketsRavelli.Application.Services.Implementations {
    public class RegistrosMedicosService : IRegistrosMedicosService {
        private readonly ApplicationDbContext _context;
        public RegistrosMedicosService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task AtualizarRegistroMedico(RegistroMedico registroMedico, RegistroMedicoInputModel registroMedicoInputModel) {
            registroMedico.EditarRegistrosMedicos(registroMedicoInputModel.Plano,
                registroMedicoInputModel.PlanoEmpresa, registroMedicoInputModel.PlanoTipo, registroMedicoInputModel.PressaoAlta,
                registroMedicoInputModel.Desmaio, registroMedicoInputModel.Cardiaco, registroMedicoInputModel.Diabetes,
                registroMedicoInputModel.Asma, registroMedicoInputModel.Alergia, registroMedicoInputModel.AlergiaQual,
                registroMedicoInputModel.Cirurgia, registroMedicoInputModel.CirurgiaQual, registroMedicoInputModel.Medicacao,
                registroMedicoInputModel.MedicacaoQual, registroMedicoInputModel.MedicacaoTempo, registroMedicoInputModel.Malestar,
                registroMedicoInputModel.MalestarQual, registroMedicoInputModel.Acompanhamento, registroMedicoInputModel.AcompanhamentoQual,
                registroMedicoInputModel.Outros);

            await _context.SaveChangesAsync();
        }

        public async Task CadastrarRegistroMedico(RegistroMedicoInputModel registroMedicoInputModel) {
            var novoRegistroMedico = new RegistroMedico(registroMedicoInputModel.IdAtleta, registroMedicoInputModel.Plano,
                                                        registroMedicoInputModel.PlanoEmpresa, registroMedicoInputModel.PlanoTipo, registroMedicoInputModel.PressaoAlta,
                                                        registroMedicoInputModel.Desmaio, registroMedicoInputModel.Cardiaco, registroMedicoInputModel.Diabetes,
                                                        registroMedicoInputModel.Asma, registroMedicoInputModel.Alergia, registroMedicoInputModel.AlergiaQual,
                                                        registroMedicoInputModel.Cirurgia, registroMedicoInputModel.CirurgiaQual, registroMedicoInputModel.Medicacao,
                                                        registroMedicoInputModel.MedicacaoQual, registroMedicoInputModel.MedicacaoTempo, registroMedicoInputModel.Malestar,
                                                        registroMedicoInputModel.MalestarQual, registroMedicoInputModel.Acompanhamento, registroMedicoInputModel.AcompanhamentoQual,
                                                        registroMedicoInputModel.Outros);

            await _context.RegistrosMedicos.AddAsync(novoRegistroMedico);
            await _context.SaveChangesAsync();
        }

        public async Task<RegistroMedico> ConsultaRegistroAtleta(string idAtleta) {
            return await _context.RegistrosMedicos
                .SingleOrDefaultAsync(m => m.IdAtleta == idAtleta);
        }
    }
}

