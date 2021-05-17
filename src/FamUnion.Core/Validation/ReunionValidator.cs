using FamUnion.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Core.Validation
{
    public class ReunionValidator
    {
        private readonly IReunionRepository _reunionRepository;

        public ReunionValidator(IReunionRepository reunionRepository)
        {
            _reunionRepository = Validator.ThrowIfNull(reunionRepository, nameof(reunionRepository));
        }

        public async Task<bool> ReunionIdExistsAsync(Guid reunionId)
        {
            var reunion = await _reunionRepository.GetReunionAsync(reunionId)
                .ConfigureAwait(continueOnCapturedContext: false);

            return reunion != null;
        }
    }
}
