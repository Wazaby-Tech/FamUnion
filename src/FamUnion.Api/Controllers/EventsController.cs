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

        [HttpGet("reunion/{reunionId}")]
        public async Task<IActionResult> GetEventsByReunion(Guid reunionId)
        {
            _logger.LogInformation($"EventsController.GetEventsByReunion|{reunionId}");
            var results = await _eventsService.GetEventsByReunionAsync(reunionId)
                .ConfigureAwait(continueOnCapturedContext: false);
            return Ok(results);
        }

        [HttpPost("update")]
        public async Task<IActionResult> SaveEvent([FromBody] Event @event)
        {
            _logger.LogInformation($"EventsController.SaveEvent|{JsonConvert.SerializeObject(@event)}");
            var result = await _eventsService.SaveEventAsync(@event)
                .ConfigureAwait(continueOnCapturedContext: false);
            return Ok(result);
        }
    }
}