using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<EventService> _logger;


        public EventService(ILogger<EventService> logger, IEventRepository eventRepository,
            IAddressRepository addressRepository)
        {
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
            _eventRepository = Validator.ThrowIfNull(eventRepository, nameof(eventRepository));
            _addressRepository = Validator.ThrowIfNull(addressRepository, nameof(addressRepository));
        }

        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            return await _eventRepository.GetEventAsync(eventId)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<IEnumerable<Event>> GetEventsByReunionIdAsync(Guid reunionId)
        {
            var events = await _eventRepository.GetEventsByReunionIdAsync(reunionId)
                .ConfigureAwait(continueOnCapturedContext: false);

            Parallel.ForEach(events, async @event =>
            {                
                @event.Location = await _addressRepository.GetEventAddressAsync(@event.Id.Value)
                    .ConfigureAwait(continueOnCapturedContext: false);
            });

            return events;
        }

        public async Task<Event> SaveEventAsync(Event @event)
        {
            var savedEvent = await _eventRepository.SaveEventAsync(@event)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (@event.Location != null)
            {
                _ = await _addressRepository.SaveEventAddressAsync(savedEvent.Id.Value, @event.Location)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            return await GetEventByIdAsync(savedEvent.Id.Value).
                ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
