using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FamUnion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private ILogger<EventsController> _logger;
        private IEventService _eventService;

        public EventsController(ILogger<EventsController> logger, IEventService eventService)
        {
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
            _eventService = Validator.ThrowIfNull(eventService, nameof(eventService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            _logger.LogInformation($"EventsController.GetEventById|{id}");
            try
            {
                var result = await _eventService.GetEventByIdAsync(id)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if(result is null)
                {
                    return NotFound($"Id: {id} not found");
                }

                return Ok(result);
            }
            catch (UnauthorizedAccessException uae)
            {
                _logger.LogWarning(uae.Message);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("reunion/{reunionId}")]
        public async Task<IActionResult> GetEventsByReunion(Guid reunionId)
        {
            _logger.LogInformation($"EventsController.GetEventsByReunion|{reunionId}");
            try
            {
                var results = await _eventService.GetEventsByReunionIdAsync(reunionId)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return Ok(results);
            }
            catch (UnauthorizedAccessException uae)
            {
                _logger.LogWarning(uae.Message);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> SaveEvent([FromBody] Event @event)
        {
            _logger.LogInformation($"EventsController.SaveEvent|{JsonConvert.SerializeObject(@event)}");
            try
            {
                var result = await _eventService.SaveEventAsync(@event)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return Ok(result);
            }
            catch(UnauthorizedAccessException uae)
            {
                _logger.LogWarning(uae.Message);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch(Exception ex)
            {
                if(!@event.IsValid())
                {
                    return BadRequest(@event);
                }

                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelEvent([FromBody] CancelRequest request)
        {
            _logger.LogInformation($"EventsController.CancelEvent|{JsonConvert.SerializeObject(request)}");
            try
            {
                await _eventService.CancelEventAsync(request)
                    .ConfigureAwait(continueOnCapturedContext: false);

                _logger.LogInformation($"Event canceled with id: '{request.EntityId}' by user id: '{request.UserId}'");
                
                return Ok();
            }
            catch(UnauthorizedAccessException uae)
            {
                _logger.LogWarning(uae.Message);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}