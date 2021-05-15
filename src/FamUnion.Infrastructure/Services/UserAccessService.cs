using FamUnion.Core.Interface.Repository;
using FamUnion.Core.Interface.Services;
using FamUnion.Core.Utility;
using FamUnion.Core.Validation;
using System;
using System.Threading.Tasks;

namespace FamUnion.Infrastructure.Services
{
    public class UserAccessService : IUserAccessService
    {
        private readonly IUserAccessRepository _userAccessRepository;
        
        public UserAccessService(IUserAccessRepository userAccessRepository)
        {
            _userAccessRepository = Validator.ThrowIfNull(userAccessRepository, nameof(userAccessRepository));
        }

        public async Task<bool> HasReadAccessToEntity(string userId, Constants.EntityType type, Guid id)
        {
            return await _userAccessRepository.HasReadAccessToEntity(userId, type, id)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        public Task<bool> HasWriteAccessToEntity(string userId, Constants.EntityType type, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
