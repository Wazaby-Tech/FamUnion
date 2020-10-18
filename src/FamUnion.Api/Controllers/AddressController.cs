using System;
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
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly IAddressRepository _addressRepository;

        public AddressController(ILogger<AddressController> logger, IAddressRepository addressRepository)
        {
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
            _addressRepository = Validator.ThrowIfNull(addressRepository, nameof(addressRepository));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(Guid id)
        {
            _logger.LogInformation($"AddressController.GetAddressById|{id}");
            try
            {
                var result = await _addressRepository.GetAddressAsync(id)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if(result is null)
                {
                    _logger.LogInformation($"GetAddressById Not Found|{id}");
                    return NotFound(id);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("reunion/{reunionId}")]
        public async Task<IActionResult> GetReunionAddress(Guid reunionId)
        {
            _logger.LogInformation($"AddressController.GetReunionAddress|{reunionId}");
            try
            {
                var result = await _addressRepository.GetReunionAddressAsync(reunionId)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if(result == null)
                {
                    return NotFound(reunionId);
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("reunion/{reunionId}")]
        public async Task<IActionResult> SaveReunionAddress(Guid reunionId, [FromBody] Address address)
        {
            _logger.LogInformation($"AddressController.SaveReunionAddress|{reunionId}|{JsonConvert.SerializeObject(address)}");
            try
            {
                var result = await _addressRepository.SaveReunionAddressAsync(reunionId, address)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEventAddress(Guid eventId)
        {
            _logger.LogInformation($"AddressController.GetEventAddress|{eventId}");
            try
            {
                var result = await _addressRepository.GetEventAddressAsync(eventId)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if(result == null)
                {
                    return NotFound(eventId);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("event/{eventId}")]
        public async Task<IActionResult> SaveEventAddress(Guid eventId, [FromBody] Address address)
        {
            _logger.LogInformation($"AddressController.SaveEventAddress|{eventId}|{JsonConvert.SerializeObject(address)}");
            try
            {
                var result = await _addressRepository.SaveEventAddressAsync(eventId, address)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}