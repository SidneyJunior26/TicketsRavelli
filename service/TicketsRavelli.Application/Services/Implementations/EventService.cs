using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Application.ViewModels.Eventos;
using TicketsRavelli.Controllers.Eventos;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public EventService(IEventRepository eventRepository, ISubscriptionRepository subscriptionRepository)
        {
            _eventRepository = eventRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task UpdateAsync(Evento evento, EventInputModel eventInputModel)
        {
            evento.Update(eventInputModel.Nome, eventInputModel.Descricao, eventInputModel.Local,
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

        public async Task<int> CreateAsync(EventInputModel eventInputModel)
        {
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

            await _eventRepository.CreateAsync(newEvent);
            await _eventRepository.SaveChangesAsync();

            return newEvent.Id;
        }

        public async Task<Evento?> GetByIdAsync(int id)
        {
            return await _eventRepository.QueryByIdAsync(id);
        }

        public async Task<List<EventoComEstatisticasInscricao>> GetAllWithStatisticsAsync()
        {
            var subscriptions = await _subscriptionRepository.QueryAllSubscriptionsWithEventAsync();

            var eventsWithStatistics = subscriptions
                        .GroupBy(i => i.IdEvento)
                        .Select(g => new EventoComEstatisticasInscricao
                        {
                            Evento = g.First().Evento,
                            QtdeInscricoesPagas = g.Count(i => i.Pago == true),
                            QtdInscricoes = g.Count()
                        })
                        .ToList();

            return eventsWithStatistics;
        }

        public async Task<List<Evento>> GetAllActivesAsync()
        {
            return await _eventRepository.QueryAllActivesAsync();
        }

        public async Task<List<Evento>> GetUpComingAsync()
        {
            return await _eventRepository.QueryUpComingAsync();
        }

        public async Task<PacotesViewModel> GetPackagesAsync(int eventoId)
        {
            var @event = await _eventRepository.QueryByIdAsync(eventoId);

            return new PacotesViewModel(@event);
        }

        public async Task<decimal> GetPackageValueAsync(int? packageId, int eventId)
        {
            var @event = await _eventRepository.QueryByIdAsync(eventId);

            decimal packageValue = 0;

            switch (packageId)
            {
                case 1:
                    if (DateTime.Today < @event.DataDesconto)
                    {
                        packageValue = @event.Valor1;
                    }
                    else if (DateTime.Today >= @event.DataDesconto && DateTime.Today < @event.DataValorNormal)
                    {
                        packageValue = @event.Valor2;
                    }
                    else if (@event.DataValorNormal != null && DateTime.Today >= @event.DataValorNormal && @event.ValorNormal != null)
                    {
                        packageValue = (decimal)@event.ValorNormal;
                    }
                    else
                    {
                        packageValue = @event.Valor2;
                    }
                    break;
                case 2:
                    if (DateTime.Today < @event.DataDesconto)
                    {
                        packageValue = @event.Pacote2V1;
                    }
                    else if (DateTime.Today >= @event.DataDesconto && DateTime.Today < @event.DataValorNormal)
                    {
                        packageValue = @event.Pacote2V2;
                    }
                    else if (DateTime.Today >= @event.DataValorNormal)
                    {
                        packageValue = @event.Pacote2V3;
                    }
                    else
                    {
                        packageValue = @event.Pacote2V2;
                    }
                    break;
                case 3:
                    if (DateTime.Today < @event.DataDesconto)
                    {
                        packageValue = @event.Pacote3V1;
                    }
                    else if (DateTime.Today >= @event.DataDesconto && DateTime.Today < @event.DataValorNormal)
                    {
                        packageValue = @event.Pacote3V2;
                    }
                    else if (DateTime.Today >= @event.DataValorNormal)
                    {
                        packageValue = @event.Pacote3V3;
                    }
                    else
                    {
                        packageValue = @event.Pacote3V2;
                    }
                    break;
                case 4:
                    if (DateTime.Today < @event.DataDesconto)
                    {
                        packageValue = @event.Pacote4V1;
                    }
                    else if (DateTime.Today >= @event.DataDesconto && DateTime.Today < @event.DataValorNormal)
                    {
                        packageValue = @event.Pacote4V2;
                    }
                    else if (DateTime.Today >= @event.DataValorNormal)
                    {
                        packageValue = @event.Pacote4V3;
                    }
                    else
                    {
                        packageValue = @event.Pacote4V2;
                    }
                    break;
                default:
                    packageValue = 0;
                    break;
            }

            return packageValue;
        }

        public async Task DeleteAsync(Evento evento)
        {
            _eventRepository.Delete(evento);

            await _eventRepository.SaveChangesAsync();
        }

        public async Task UpdateImageAsync(Evento evento, string nome)
        {
            evento.UpdateNameImageEvent(nome);

            await _eventRepository.SaveChangesAsync();
        }
    }
}