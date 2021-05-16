using FamUnion.Core.Interface;
using FamUnion.Core.Interface.Services;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using FamUnion.Core.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Services
{
    public class ReunionService : IReunionService
    {
        private readonly IReunionRepository _reunionRepository;
        private readonly IUserAccessService _userAccessService;
        private readonly IUserRepository _userRepository;
        private readonly IAddressService _addressService;
        private readonly IEventService _eventService;
        private readonly IInviteService _inviteService;

        public ReunionService(IReunionRepository reunionRepository, IUserRepository userRepository, 
            IUserAccessService userAccessService, IAddressService addressService, IEventService eventService,
            IInviteService inviteService)
        {
            _reunionRepository = Validator.ThrowIfNull(reunionRepository, nameof(reunionRepository));
            _userRepository = Validator.ThrowIfNull(userRepository, nameof(userRepository));
            _userAccessService = Validator.ThrowIfNull(userAccessService, nameof(userAccessService));
            _addressService = Validator.ThrowIfNull(addressService, nameof(addressService));
            _eventService = Validator.ThrowIfNull(eventService, nameof(eventService));
            _inviteService = Validator.ThrowIfNull(inviteService, nameof(inviteService));
        }

        public async Task<Reunion> GetReunionAsync(Guid id)
        {
            var reunion = await _reunionRepository.GetReunionAsync(id).
                ConfigureAwait(continueOnCapturedContext: false);

            await PopulateDependentProperties(reunion.Yield())
                .ConfigureAwait(continueOnCapturedContext: false);

            return reunion;
        }

        public async Task<IEnumerable<Reunion>> GetReunionsAsync()
        {
            var reunions = await _reunionRepository.GetReunionsAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            await PopulateDependentProperties(reunions)
                .ConfigureAwait(continueOnCapturedContext: false);

            return reunions;
        }

        public async Task<IEnumerable<Reunion>> GetManageReunionsAsync(string userId)
        {
            var reunions = await _reunionRepository.GetManageReunionsAsync(userId)
                .ConfigureAwait(continueOnCapturedContext: false);

            await PopulateDependentProperties(reunions)
                .ConfigureAwait(continueOnCapturedContext: false);

            return reunions;
        }

        public async Task<Reunion> SaveReunionAsync(Reunion reunion)
        {
            if(!await _userRepository.ValidateUserIdAsync(reunion.ActionUserId))
            {
                throw new Exception($"Invalid user id when saving reunion: '{reunion.ActionUserId}'");
            }

            if(!await CheckUserWriteAccess(reunion.ActionUserId, reunion.Id.Value))
            {
                throw new UnauthorizedAccessException($"User {reunion.ActionUserId} does not have access to write for reunion {reunion.Id.Value}");
            }

            var savedReunion = await _reunionRepository.SaveReunionAsync(reunion)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (reunion.Location != null)
            {
                reunion.Location.ActionUserId = reunion.ActionUserId;

                var addrRequest = new SaveReunionAddressRequest(savedReunion.Id.Value, reunion.Location);

                savedReunion.Location = await _addressService.SaveEntityAddressAsync(addrRequest)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            return await GetReunionAsync(savedReunion.Id.Value)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task CancelReunionAsync(CancelRequest request)
        {
            if (!await _userRepository.ValidateUserIdAsync(request.UserId))
            {
                throw new Exception($"Invalid user id when saving reunion: '{request.EntityId}'");
            }

            if (!await CheckUserWriteAccess(request.UserId, request.EntityId))
            {
                throw new UnauthorizedAccessException($"User {request.UserId} does not have access to write for reunion {request.EntityId}");
            }

            await _reunionRepository.CancelReunionAsync(request)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task AddReunionOrganizer(OrganizerRequest request)
        {
            if(request.Action != Constants.OrganizerAction.Add)
            {
                throw new Exception($"Invalid action specified for AddReunionOrganizer|{request.Action}");
            }

            if (!await _userRepository.ValidateUserIdAsync(request.UserId) || !await _userRepository.ValidateUserIdAsync(request.ActionUserId))
            {
                throw new Exception($"Invalid user id when saving reunion: '{request.ReunionId}'");
            }

            if (!await CheckUserWriteAccess(request.ActionUserId, request.ReunionId))
            {
                throw new UnauthorizedAccessException($"User {request.UserId} does not have access to write for reunion {request.ReunionId}");
            }

            await _reunionRepository.AddReunionOrganizer(request.ReunionId, request.UserId)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task RemoveReunionOrganizer(OrganizerRequest request)
        {
            if (request.Action != Constants.OrganizerAction.Remove)
            {
                throw new Exception($"Invalid action specified for RemoveReunionOrganizer|{request.Action}");
            }

            if (!await _userRepository.ValidateUserIdAsync(request.UserId) || !await _userRepository.ValidateUserIdAsync(request.ActionUserId))
            {
                throw new Exception($"Invalid user id when saving reunion: '{request.ReunionId}'");
            }

            if (!await CheckUserWriteAccess(request.ActionUserId, request.ReunionId))
            {
                throw new UnauthorizedAccessException($"User {request.UserId} does not have access to write for reunion {request.ReunionId}");
            }

            await _reunionRepository.RemoveReunionOrganizer(request.ReunionId, request.UserId)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task<bool> CheckUserWriteAccess(string userId, Guid? id)
        {
            return !id.HasValue || await _userAccessService.HasWriteAccessToEntity(userId, Constants.EntityType.Reunion, id.Value);
        }

        private async Task PopulateDependentProperties(IEnumerable<Reunion> reunions)
        {
            await PopulateAddresses(reunions)
                .ConfigureAwait(continueOnCapturedContext: false);

            await PopulateEvents(reunions)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        private Task PopulateEvents(IEnumerable<Reunion> reunions)
        {
            Parallel.ForEach(reunions, async reunion =>
            {
                if (reunion != null && reunion.Id.HasValue)
                {
                    reunion.Events = await _eventService.GetEventsByReunionIdAsync(reunion.Id.Value)
                    .ConfigureAwait(continueOnCapturedContext: false);
                }
            });

            return Task.CompletedTask;
        }

        private Task PopulateAddresses(IEnumerable<Reunion> reunions)
        {
            Parallel.ForEach(reunions, async reunion =>
            {
                if (reunion != null && reunion.Id.HasValue)
                {
                    var addrRequest = new GetReunionAddressRequest(reunion.Id.Value);

                    reunion.Location = await _addressService.GetEntityAddressAsync(addrRequest)
                    .ConfigureAwait(continueOnCapturedContext: false);

                }
            });

            return Task.CompletedTask;
        }
    }
}
