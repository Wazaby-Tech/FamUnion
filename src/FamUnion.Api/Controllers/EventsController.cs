using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FamUnion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private ILogger<EventsController> _logger;
        private IEventService _eventsService;

        public EventsController(ILogger<EventsController> logger, IEventService eventService)
        {
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
            _eventsService = Validator.ThrowIfNull(eventService, nameof(eventService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            _logger.LogInformation($"EventsController.GetEventById|{id}");
            try
            {
                var result = await _eventsService.GetEventByIdAsync(id)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if(result is null)
                {
                    return NotFound($"Id: {id} not found");
                }

                return Ok(result);
            }
            catch(Exception ex)
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
                var results = await _eventsService.GetEventsByReunionIdAsync(reunionId)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return Ok(results);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> SaveEvent([FromBody] Event @event)
        {
            _logger.LogInformation($"EventsController.SaveEvent|{JsonConvert.SerializeObject(@event)}");
            try
            {
                var result = await _eventsService.SaveEventAsync(@event)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return Ok(result);
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
    }
}