using System;
using System.Threading.Tasks;
using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using FamUnion.Core.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp.Serialization;

namespace FamUnion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReunionsController : ControllerBase
    {
        private readonly IReunionService _reunionService;
        private readonly ILogger<ReunionsController> _logger;

        public ReunionsController(IReunionService reunionService, ILogger<ReunionsController> logger)
        {
            _reunionService = Validator.ThrowIfNull(reunionService, nameof(reunionService));
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetReunions()
        {
            _logger.LogInformation("ReunionsController.GetReunions()");
            try
            {
                var result = await _reunionService.GetReunionsAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("manage/{userId}")]
        public async Task<IActionResult> GetManageReunions(string userId)
        {
            _logger.LogInformation("ReunionsController.GetManageReunions");
            try
            {
                var result = await _reunionService.GetManageReunionsAsync(userId)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReunion(Guid id)
        {
            _logger.LogInformation($"ReunionsController.GetReunion|{id}");
            try
            {
                var result = await _reunionService.GetReunionAsync(id)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if (result is null)
                    return NotFound($"Id: {id} was not found");

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> NewReunion([FromBody] NewReunionRequest request)
        {
            _logger.LogInformation($"{GetType()}.NewReunion|{JsonConvert.SerializeObject(request)}");
            Reunion reunion = null;
            try
            {
                reunion = NewReunionRequestMapper.Map(request);

                if (!reunion.IsValid())
                {
                    return BadRequest(request);
                }

                var result = await _reunionService.SaveReunionAsync(reunion)
                    .ConfigureAwait(continueOnCapturedContext: false);

                await _reunionService.AddReunionOrganizer(OrganizerRequest.AddOrganizerRequest(reunion.Id.Value, reunion.ActionUserId, reunion.ActionUserId))
                    .ConfigureAwait(continueOnCapturedContext: false);

                var resp = CreatedAtAction("GetReunion",
                    routeValues: new { id = result.Id.Value },
                    value: result);
                resp.ContentTypes.Add(ContentType.Json);
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveReunion([FromBody] Reunion reunion)
        {
            _logger.LogInformation($"ReunionsController.SaveReunion|{JsonConvert.SerializeObject(reunion)}");
            try
            {
                if(!reunion.Id.HasValue)
                {
                    throw new Exception("Reunion doesn't have a valid ID");
                }

                if(string.IsNullOrWhiteSpace(reunion.ActionUserId))
                {
                    reunion.ActionUserId = Helpers.GetUserId(HttpContext.User);
                }

                var result = await _reunionService.SaveReunionAsync(reunion)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                if (!reunion.IsValid())
                {
                    return BadRequest(reunion);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelReunion([FromBody] CancelRequest request)
        {
            _logger.LogInformation($"ReunionsController.CancelReunion|{request}");
            try
            {
                await _reunionService.CancelReunionAsync(request)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("organizers")]
        public async Task<IActionResult> OrganizerOperations([FromBody] OrganizerRequest request)
        {
            _logger.LogInformation($"ReunionsController.GetOrganizers|{request}");
            try
            {
                switch(request.Action)
                {
                    case Constants.OrganizerAction.List:
                        var results = await _reunionService.GetReunionOrganizers(request)
                            .ConfigureAwait(continueOnCapturedContext: false);
                        return Ok(results);

                    case Constants.OrganizerAction.Add:
                        await _reunionService.AddReunionOrganizer(request)
                            .ConfigureAwait(continueOnCapturedContext: false);
                        return Ok();

                    case Constants.OrganizerAction.Remove:
                        await _reunionService.RemoveReunionOrganizer(request)
                            .ConfigureAwait(continueOnCapturedContext: false);
                        return Ok();

                    default:
                        throw new ArgumentException("Invalid action specified");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}