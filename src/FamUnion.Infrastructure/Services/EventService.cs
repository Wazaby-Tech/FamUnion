using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using FamUnion.Core.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Services
{
    public class EventService : IEventService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IAddressService _addressService;
        private readonly ILogger<EventService> _logger;


        public EventService(ILogger<EventService> logger, IUserRepository userRepository, 
            IEventRepository eventRepository, IAddressService addressService)
        {
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
            _userRepository = Validator.ThrowIfNull(userRepository, nameof(userRepository));
            _eventRepository = Validator.ThrowIfNull(eventRepository, nameof(eventRepository));
            _addressService = Validator.ThrowIfNull(addressService, nameof(addressService));
        }

        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            var savedEvent = await _eventRepository.GetEventAsync(eventId)
                .ConfigureAwait(continueOnCapturedContext: false);

            await PopulateAddresses(savedEvent.Yield()).ConfigureAwait(continueOnCapturedContext: false);

            return savedEvent;
        }

        public async Task<IEnumerable<Event>> GetEventsByReunionIdAsync(Guid reunionId)
        {
            var events = await _eventRepository.GetEventsByReunionIdAsync(reunionId)
                .ConfigureAwait(continueOnCapturedContext: false);

            await PopulateAddresses(events).ConfigureAwait(continueOnCapturedContext: false);

            return events;
        }

        public async Task<Event> SaveEventAsync(Event @event)
        {
            if (!await _userRepository.ValidateUserIdAsync(@event.ActionUserId))
            {
                throw new Exception($"Invalid user id when saving event: '{@event.ActionUserId}'");
            }

            var savedEvent = await _eventRepository.SaveEventAsync(@event)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (@event.Location != null)
            {
                @event.Location.ActionUserId = @event.ActionUserId;
                _ = await _addressService.SaveEntityAddressAsync(new SaveEventAddressRequest(savedEvent.Id.Value, @event.Location))
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            return await GetEventByIdAsync(savedEvent.Id.Value).
                ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task DeleteEventAsync(Guid eventId)
        {
            await _eventRepository.DeleteEventAsync(eventId)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        #region Helper Methods

        private Task PopulateAddresses(IEnumerable<Event> events)
        {
            Parallel.ForEach(events, async @event =>
            {
                if (@event != null && @event.Id.HasValue)
                {
                    var addrRequest = new GetEventAddressRequest(@event.Id.Value);

                    @event.Location = await _addressService.GetEntityAddressAsync(addrRequest)
                    .ConfigureAwait(continueOnCapturedContext: false);

                }
            });

            return Task.CompletedTask;
        }

        #endregion
    }
}
