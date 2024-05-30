using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Application.ViewModels.Eventos;
using TicketsRavelli.Controllers.Eventos;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class EventService : IEventService {
        private readonly IEventRepository _eventRepository;
        private readonly ISubscriptionService _subscriptionService;

        public EventService(IEventRepository eventRepository, ISubscriptionService subscriptionService)
        {
            _eventRepository = eventRepository;
            _subscriptionService = subscriptionService;
        }

        public async Task UpdateEventAsync(Evento evento, EventInputModel eventInputModel) {
            evento.UpdateEvent(eventInputModel.Nome, eventInputModel.Descricao, eventInputModel.Local,
                eventInputModel.Data, eventInputModel.DataIniInscricao, eventInputModel.DataFimInscricao,
                eventInputModel.DataDesconto, eventInputModel.DataValorNormal, eventInputModel.Valor1,
                eventInputModel.Valor2, eventInputModel.ValorNormal, eventInputModel.Pacote2V1,
                eventInputModel.Pacote2V2, eventInputModel.Pacote2V3, eventInputModel.Pacote3V1,
                eventInputModel.Pacote3V2, eventInputModel.Pacote3V3, eventInputModel.Pacote4V1,
                eventInputModel.Pacote4V2, eventInputModel.Pacote4V3, eventInputModel.Pacote1Desc,
                eventInputModel.Pacote2Desc, eventInputModel.Pacote3Desc, eventInputModel.Pacote4Desc,
                eventInputModel.Pacote1Ativo, eventInputModel.Pacote2Ativo, eventInputModel.Pacote3Ativo,
                eventInputModel.Pacote4Ativo, eventInputModel.Categoria, eventInputModel.BoletoInf1,
                eventInputModel.BoletoInf2, eventInputModel.BoletoInf3, eventInputModel.BoletoInstrucao1,
                eventInputModel.BoletoInstrucao2, eventInputModel.BoletoInstrucao3, eventInputModel.ObsTela,
                eventInputModel.TxtEmailCadastro, eventInputModel.TxtEmailBaixa, eventInputModel.AtivaInscricao,
                eventInputModel.AtivaEvento, eventInputModel.AtivaAlteracaoInscricao, eventInputModel.EventoTipo, eventInputModel.Pacote1V1Pseg,
                eventInputModel.Pacote1V2Pseg, eventInputModel.Pacote1V3Pseg, eventInputModel.Pacote2V1Pseg,
                eventInputModel.Pacote2V2Pseg, eventInputModel.Pacote2V3Pseg, eventInputModel.Pacote3V1Pseg,
                eventInputModel.Pacote3V2Pseg, eventInputModel.Pacote3V3Pseg, eventInputModel.Pacote4V1Pseg,
                eventInputModel.Pacote4V2Pseg, eventInputModel.Pacote4V3Pseg);

            await _eventRepository.SaveChangesAsync();
        }

        public async Task<int> CreateEventAsync(EventInputModel eventInputModel) {
            var newEvent = new Evento(eventInputModel.Nome, eventInputModel.Descricao, eventInputModel.Local,
                eventInputModel.Data, eventInputModel.DataIniInscricao, eventInputModel.DataFimInscricao,
                eventInputModel.DataDesconto, eventInputModel.DataValorNormal, eventInputModel.Valor1,
                eventInputModel.Valor2, eventInputModel.ValorNormal, eventInputModel.Pacote2V1,
                eventInputModel.Pacote2V2, eventInputModel.Pacote2V3, eventInputModel.Pacote3V1,
                eventInputModel.Pacote3V2, eventInputModel.Pacote3V3, eventInputModel.Pacote4V1,
                eventInputModel.Pacote4V2, eventInputModel.Pacote4V3, eventInputModel.Pacote1Desc,
                eventInputModel.Pacote2Desc, eventInputModel.Pacote3Desc, eventInputModel.Pacote4Desc,
                eventInputModel.Pacote1Ativo, eventInputModel.Pacote2Ativo, eventInputModel.Pacote3Ativo,
                eventInputModel.Pacote4Ativo, eventInputModel.Categoria, eventInputModel.BoletoInf1,
                eventInputModel.BoletoInf2, eventInputModel.BoletoInf3, eventInputModel.BoletoInstrucao1,
                eventInputModel.BoletoInstrucao2, eventInputModel.BoletoInstrucao3, eventInputModel.ObsTela,
                eventInputModel.TxtEmailCadastro, eventInputModel.TxtEmailBaixa, eventInputModel.AtivaInscricao,
                eventInputModel.AtivaEvento, eventInputModel.AtivaAlteracaoInscricao, eventInputModel.EventoTipo, eventInputModel.Pacote1V1Pseg,
                eventInputModel.Pacote1V2Pseg, eventInputModel.Pacote1V3Pseg, eventInputModel.Pacote2V1Pseg,
                eventInputModel.Pacote2V2Pseg, eventInputModel.Pacote2V3Pseg, eventInputModel.Pacote3V1Pseg,
                eventInputModel.Pacote3V2Pseg, eventInputModel.Pacote3V3Pseg, eventInputModel.Pacote4V1Pseg,
                eventInputModel.Pacote4V2Pseg, eventInputModel.Pacote4V3Pseg);

            await _eventRepository.CreateEventAsync(newEvent);
            await _eventRepository.SaveChangesAsync();

            return newEvent.Id;
        }

        public async Task<Evento?> GetEventByIdAsync(int id) {
            return await _eventRepository.QueryEventByIdAsync(id);
        }

        public async Task<List<EventoComEstatisticasInscricao>> GetEventsWithStatisticsAsync() {
            var subscriptions = await _subscriptionService.GetAllSubscriptionsWithEventAsync();

            var eventsWithStatistics = subscriptions
                        .GroupBy(i => i.IdEvento)
                        .Select(g => new EventoComEstatisticasInscricao {
                            Evento = g.First().Evento,
                            QtdeInscricoesPagas = g.Count(i => i.Pago == true),
                            QtdInscricoes = g.Count()
                        })
                        .ToList();

            return eventsWithStatistics;
        }

        public async Task<List<Evento>> GetAllEventsActivesAsync() {
            return await _eventRepository.QueryAllEventsActivesAsync();
        }

        public async Task<List<Evento>> GetUpComingEventsAsync() {
            return await _eventRepository.QueryUpComingEventsAsync();
        }

        public async Task<PacotesViewModel> GetPackagesAsync(int eventoId) {
            var eventById = await _eventRepository.QueryEventByIdAsync(eventoId);

            return new PacotesViewModel(eventById);
        }

        public async Task<decimal> GetPackageValueAsync(int? packageId, int eventId) {
            var eventoById = await _eventRepository.QueryEventByIdAsync(eventId);

            decimal packageValue = 0;

            switch (packageId) {
                case 1:
                    if (DateTime.Today < eventoById.DataDesconto) {
                        packageValue = eventoById.Valor1;
                    } else if (DateTime.Today >= eventoById.DataDesconto && DateTime.Today < eventoById.DataValorNormal) {
                        packageValue = eventoById.Valor2;
                    } else if (eventoById.DataValorNormal != null && DateTime.Today >= eventoById.DataValorNormal && eventoById.ValorNormal != null) {
                        packageValue = (decimal)eventoById.ValorNormal;
                    } else {
                        packageValue = eventoById.Valor2;
                    }
                    break;
                case 2:
                    if (DateTime.Today < eventoById.DataDesconto) {
                        packageValue = eventoById.Pacote2V1;
                    } else if (DateTime.Today >= eventoById.DataDesconto && DateTime.Today < eventoById.DataValorNormal) {
                        packageValue = eventoById.Pacote2V2;
                    } else if (DateTime.Today >= eventoById.DataValorNormal) {
                        packageValue = eventoById.Pacote2V3;
                    } else {
                        packageValue = eventoById.Pacote2V2;
                    }
                    break;
                case 3:
                    if (DateTime.Today < eventoById.DataDesconto) {
                        packageValue = eventoById.Pacote3V1;
                    } else if (DateTime.Today >= eventoById.DataDesconto && DateTime.Today < eventoById.DataValorNormal) {
                        packageValue = eventoById.Pacote3V2;
                    } else if (DateTime.Today >= eventoById.DataValorNormal) {
                        packageValue = eventoById.Pacote3V3;
                    } else {
                        packageValue = eventoById.Pacote3V2;
                    }
                    break;
                case 4:
                    if (DateTime.Today < eventoById.DataDesconto) {
                        packageValue = eventoById.Pacote4V1;
                    } else if (DateTime.Today >= eventoById.DataDesconto && DateTime.Today < eventoById.DataValorNormal) {
                        packageValue = eventoById.Pacote4V2;
                    } else if (DateTime.Today >= eventoById.DataValorNormal) {
                        packageValue = eventoById.Pacote4V3;
                    } else {
                        packageValue = eventoById.Pacote4V2;
                    }
                    break;
                default:
                    packageValue = 0;
                    break;
            }

            return packageValue;
        }

        public async Task DeleteEventAsync(Evento evento) {
            await _eventRepository.DeleteEventAsync(evento);

            await _eventRepository.SaveChangesAsync();
        }

        public async Task UpdateImageAsync(Evento evento, string nome) {
            evento.UpdateNameImageEvent(nome);

            await _eventRepository.SaveChangesAsync();
        }
    }
}