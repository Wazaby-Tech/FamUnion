using System;
using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Validation;
using Microsoft.AspNetCore.Mvc;

namespace FamUnion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReunionsController : ControllerBase
    {
        private readonly IReunionRepository _reunionRepository;

        public ReunionsController(IReunionRepository reunionRepository)
        {
            _reunionRepository = Validator.ThrowIfNull(reunionRepository, nameof(reunionRepository));
        }

        [HttpGet("list")]
        public IActionResult GetReunions()
        {
            var result = _reunionRepository.GetReunions();
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetReunion(Guid id)
        {
            var result = _reunionRepository.GetReunion(id);
            return new OkObjectResult(result);
        }

        [HttpPost("save")]
        public IActionResult SaveReunion([FromBody] Reunion reunion)
        {
            var result = _reunionRepository.SaveReunion(reunion);
            return new OkObjectResult(result);
        }
    }
}