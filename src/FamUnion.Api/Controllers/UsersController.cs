using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Validation;
using log4net.Core;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
            _userRepository = Validator.ThrowIfNull(userRepository, nameof(userRepository));
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> FindUserByEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userRepository.GetUserByEmailAsync(email)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if(result is null)
                {
                    _logger.LogInformation($"User with email: '{email}' was searched and not found");
                    var user = new User() { Email = email };
                    return Ok(user);
                }

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            try
            {
                var result = await _userRepository.GetUserByIdAsync(id)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if(result is null)
                {
                    _logger.LogInformation($"User with id: '{id}' was searched and not found");
                    var user = new User() { UserId = id };
                    return Ok(user);
                }

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser([FromBody] User user)
        {
            try
            {
                if(!user.IsValid())
                {
                    return BadRequest();
                }

                var result = await _userRepository.SaveUserAsync(user)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
