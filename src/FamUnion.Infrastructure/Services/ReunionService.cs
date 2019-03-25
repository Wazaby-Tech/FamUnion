using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamUnion.Infrastructure.Services
{
    public class ReunionService : IReunionService
    {
        private readonly IReunionRepository _reunionRepository;

        public ReunionService(IReunionRepository reunionRepository)
        {
            _reunionRepository = Validator.ThrowIfNull(reunionRepository, nameof(reunionRepository));
        }

        public Reunion GetReunion(Guid id)
        {
            var reunion = _reunionRepository.GetReunion(id);

            return reunion;
        }

        public IEnumerable<Reunion> GetReunions()
        {
            var reunions = _reunionRepository.GetReunions();

            return reunions;
        }

        public Reunion SaveReunion(Reunion reunion)
        {
            var savedReunion = _reunionRepository.SaveReunion(reunion);

            return savedReunion;
        }
    }
}
