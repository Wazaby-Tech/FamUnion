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
    public class InvitesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IInviteService _inviteService;

        public InvitesController(ILogger<InvitesController> logger, IInviteService inviteService)
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
    }
}
