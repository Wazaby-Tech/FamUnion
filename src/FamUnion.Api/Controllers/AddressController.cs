using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamUnion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = Validator.ThrowIfNull(addressRepository, nameof(addressRepository));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(Guid id)
        {
            var result = await _addressRepository.GetAddressAsync(id)
                .ConfigureAwait(continueOnCapturedContext: false);
            return new OkObjectResult(result);
        }

        [HttpGet("reunion/{reunionId}")]
        public async Task<IActionResult> GetReunionAddress(Guid reunionId)
        {
            var result = await _addressRepository.GetReunionAddressAsync(reunionId)
                .ConfigureAwait(continueOnCapturedContext: false);

            return new OkObjectResult(result);
        }

        [HttpPost("reunion/{reunionId}")]
        public async Task<IActionResult> SaveReunionAddress(Guid reunionId, [FromBody] Address address)
        {
            var result = await _addressRepository.SaveReunionAddressAsync(reunionId, address)
                .ConfigureAwait(continueOnCapturedContext: false);

            return new OkObjectResult(result);
        }
    }
}