using FamUnion.Core.Interface.Services;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FamUnion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAttendeeService _attendeeService;

        public AttendeesController(ILogger<AttendeesController> logger, IAttendeeService attendeeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _attendeeService = attendeeService ?? throw new ArgumentNullException(nameof(attendeeService));
        }

        [HttpGet("{inviteInfo}")]
        public async Task<IActionResult> Invite(string inviteInfo)
        {
            try
            {
                InviteInfo invite = InviteInfo.Decode(inviteInfo);

                if(!invite.IsValid())
                {
                    return BadRequest();
                }

                var reunionInvite = await _attendeeService.GetInviteAsync(invite)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if (reunionInvite == null)
                {
                    return NotFound();
                }
                
                return Ok(reunionInvite);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddInvites([FromBody]BulkAttendeeRequest invites)
        {
            try
            {
                await _attendeeService.AddAttendees(invites)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("reunion/{reunionId}")]
        public async Task<IActionResult> GetAttendeesByReunionId(string reunionId)
        {
            try
            {
                if(!Guid.TryParse(reunionId, out Guid reunionGuid))
                {
                    throw new ArgumentException($"reunionId '{reunionId}' is invalid");
                }

                var results = await _attendeeService.GetAttendeesByReunion(reunionGuid)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return Ok(results);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
