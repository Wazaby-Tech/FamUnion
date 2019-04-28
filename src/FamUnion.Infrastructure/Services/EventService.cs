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

        public async Task<IEnumerable<Event>> GetEventsByReunionAsync(Guid reunionId)
        {
            var events = await _eventRepository.GetEventsByReunionAsync(reunionId)
                .ConfigureAwait(continueOnCapturedContext: false);

            Parallel.ForEach(events, async @event =>
            {
                @event.Location = await _addressRepository.GetEventAddressAsync(@event.AddressId)
                    .ConfigureAwait(continueOnCapturedContext: false);
            });

            return events;
        }

        public async Task<Event> SaveEventAsync(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
