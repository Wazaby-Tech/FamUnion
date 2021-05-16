using FamUnion.Core.Interface.Repository;
using FamUnion.Core.Interface.Services;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using FamUnion.Core.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Services
{
    public class InviteService : IInviteService
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly ReunionValidator _reunionValidator;
        private readonly ILogger<InviteService> _logger;
        public InviteService(ILogger<InviteService> logger, IUserAccessRepository userAccessRepository,
            ReunionValidator reunionValidator, IInviteRepository inviteRepository)
        {
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
            _userAccessRepository = Validator.ThrowIfNull(userAccessRepository, nameof(userAccessRepository));
            _reunionValidator = Validator.ThrowIfNull(reunionValidator, nameof(reunionValidator));
            _inviteRepository = Validator.ThrowIfNull(inviteRepository, nameof(inviteRepository));
        }

        public async Task<IEnumerable<AttendeeInvite>> GetInvitesByReunion(Guid reunionId)
        {
            return await _inviteRepository.GetInvitesByReunion(reunionId)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task CreateInvites(BulkInviteRequest request)
        {
            var requests = request.InviteRequests
                .Where(r => _userAccessRepository.HasWriteAccessToEntity(request.UserId, Constants.EntityType.Reunion, r.ReunionId).Result)
                .Where(r => _reunionValidator.ReunionIdExistsAsync(r.ReunionId).Result);

            request.InviteRequests = requests;

            await _inviteRepository.CreateInvites(request)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<AttendeeInvite> GetInviteAsync(InviteInfo inviteInfo)
        {
            return await _inviteRepository.GetInviteAsync(inviteInfo)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task InviteResponseAsync(Guid inviteId, Constants.InviteResponseStatus status)
        {
            await _inviteRepository.InviteResponseAsync(inviteId, status)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
