using FamUnion.Core.Interface.Services;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
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
        private readonly IInviteService _inviteService;

        public AttendeesController(ILogger<AttendeesController> logger, IInviteService inviteService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _inviteService = inviteService ?? throw new ArgumentNullException(nameof(inviteService));
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

                var reunionInvite = await _inviteService.GetInviteAsync(invite)
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
                return StatusCode(503);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddInvites([FromBody]BulkInviteRequest invites)
        {
            try
            {
                await _inviteService.CreateInvites(invites)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(503);
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

                var results = await _inviteService.GetInvitesByReunion(reunionGuid)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return Ok(results);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(503);
            }
        }
    }
}
