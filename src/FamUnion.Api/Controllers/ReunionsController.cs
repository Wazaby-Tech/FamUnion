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
        private readonly IReunionService _reunionService;

        public ReunionsController(IReunionService reunionService)
        {
            _reunionService = Validator.ThrowIfNull(reunionService, nameof(reunionService));
        }

        [HttpGet("list")]
        public IActionResult GetReunions()
        {
            var result = _reunionService.GetReunions();
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetReunion(Guid id)
        {
            var result = _reunionService.GetReunion(id);
            return new OkObjectResult(result);
        }

        [HttpPost("save")]
        public IActionResult SaveReunion([FromBody] Reunion reunion)
        {
            var result = _reunionService.SaveReunion(reunion);
            return new OkObjectResult(result);
        }
    }
}