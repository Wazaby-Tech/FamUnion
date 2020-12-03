using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Infrastructure.Services
{

    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger _logger;

        public AddressService(IAddressRepository addressRepository, ILogger<AddressService> logger)
        {
            _addressRepository = Validator.ThrowIfNull(addressRepository, nameof(addressRepository));
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
        }

        public async Task<Address> GetAddressByIdAsync(Guid addressId)
        {
            _logger.LogInformation($"{GetType()}.GetAddressByIdAsync|{JsonConvert.SerializeObject(addressId)}");
            try
            {
                return await _addressRepository.GetAddressAsync(addressId)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<Address> GetEntityAddressAsync(GetEntityAddressRequest request)
        {
            _logger.LogInformation($"{GetType()}.GetEntityAddressAsync|{JsonConvert.SerializeObject(request)}");
            try
            {
                switch(request.EntityType)
                {
                    case AddressEntityType.Reunion:
                        return await _addressRepository.GetReunionAddressAsync(request.EntityId).ConfigureAwait(continueOnCapturedContext: false);

                    case AddressEntityType.Event:
                        return await _addressRepository.GetEventAddressAsync(request.EntityId).ConfigureAwait(continueOnCapturedContext: false);
                    
                    default:
                        throw new NotSupportedException($"Entity Type not supported|{request.EntityType}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<Address> SaveEntityAddressAsync(SaveEntityAddressRequest request)
        {
            _logger.LogInformation($"{GetType()}.SaveEntityAddressAsync|{JsonConvert.SerializeObject(request)}");
            try
            {
                switch(request.EntityType)
                {
                    case AddressEntityType.Reunion:
                        return await _addressRepository.SaveReunionAddressAsync(request.EntityId, request.Address);

                    case AddressEntityType.Event:
                        return await _addressRepository.SaveEventAddressAsync(request.EntityId, request.Address);

                    default:
                        throw new NotSupportedException($"Entity Type not supported|{request.EntityType}");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
