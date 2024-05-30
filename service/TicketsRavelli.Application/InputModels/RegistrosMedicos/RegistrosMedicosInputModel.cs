using System;
namespace TicketsRavelli.Controllers.RegistrosMedicos;

public record MedicalDataInputModel(string IdAtleta, int Plano, string? PlanoEmpresa, string? PlanoTipo,
                                      int PressaoAlta, int Desmaio, int Cardiaco, int Diabetes,
                                      int Asma, int Alergia, string? AlergiaQual, int Cirurgia,
                                      string? CirurgiaQual, int Medicacao, string? MedicacaoQual,
                                      string? MedicacaoTempo, int Malestar, string? MalestarQual,
                                      int Acompanhamento, string? AcompanhamentoQual, string? Outros);
