using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetReunions()
        {
            var result = await _reunionService.GetReunionsAsync()
                .ConfigureAwait(continueOnCapturedContext: false);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReunion(Guid id)
        {
            var result = await _reunionService.GetReunionAsync(id)
                .ConfigureAwait(continueOnCapturedContext: false);
            return new OkObjectResult(result);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveReunion([FromBody] Reunion reunion)
        {
            var result = await _reunionService.SaveReunionAsync(reunion)
                .ConfigureAwait(continueOnCapturedContext: false);
            return new OkObjectResult(result);
        }
    }
}