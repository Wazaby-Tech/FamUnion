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
    public class AttendeeService : IAttendeeService
    {
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly ReunionValidator _reunionValidator;
        private readonly ILogger<AttendeeService> _logger;
        public AttendeeService(ILogger<AttendeeService> logger, IUserAccessRepository userAccessRepository,
            ReunionValidator reunionValidator, IAttendeeRepository attendeeRepository)
        {
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
            _userAccessRepository = Validator.ThrowIfNull(userAccessRepository, nameof(userAccessRepository));
            _reunionValidator = Validator.ThrowIfNull(reunionValidator, nameof(reunionValidator));
            _attendeeRepository = Validator.ThrowIfNull(attendeeRepository, nameof(attendeeRepository));
        }

        public async Task<IEnumerable<AttendeeInvite>> GetAttendeesByReunion(Guid reunionId)
        {
            return await _attendeeRepository.GetAttendeesByReunion(reunionId)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task AddAttendees(BulkAttendeeRequest request)
        {
            var requests = request.InviteRequests
                .Where(r => _userAccessRepository.HasWriteAccessToEntity(request.UserId, Constants.EntityType.Reunion, r.ReunionId).Result)
                .Where(r => _reunionValidator.ReunionIdExistsAsync(r.ReunionId).Result);

            request.InviteRequests = requests;

            await _attendeeRepository.AddAttendees(request)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<AttendeeInvite> GetInviteAsync(InviteInfo inviteInfo)
        {
            return await _attendeeRepository.GetInviteAsync(inviteInfo)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task AttendeeResponseAsync(Guid inviteId, Constants.AttendeeResponseStatus status)
        {
            await _attendeeRepository.AttendeeResponseAsync(inviteId, status)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
