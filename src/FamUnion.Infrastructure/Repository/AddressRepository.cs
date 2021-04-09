using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Infrastructure.Repository
{
    public class AddressRepository : DbAccess<Address>, IAddressRepository
    {
        public AddressRepository(string connection)
            : base(connection)
        {

        }

        public async Task<Address> GetAddressAsync(Guid id)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("id", id.ToString());

            return (await ExecuteStoredProc("[dbo].[spGetAddressById]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }

        public async Task<Address> GetEventAddressAsync(Guid eventId)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("eventId", eventId.ToString());
            return (await ExecuteStoredProc("[dbo].[spGetAddressByEventId]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }

        public Task<Address> GetLodgingAddressAsync(Guid lodgingId)
        {
            throw new NotImplementedException();
        }

        public async Task<Address> GetReunionAddressAsync(Guid reunionId)
        {
            ParameterDictionary parameters = ParameterDictionary.Single("entityTypeId", (int)EntityType.Reunion);
            parameters.AddParameter("entityId", reunionId.ToString());
            return (await ExecuteStoredProc("[dbo].[spGetAddressByEntityTypeAndId]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }

        public async Task<Address> SaveEventAddressAsync(Guid eventId, Address address)
        {
            Address currentAddress = await GetEventAddressAsync(eventId)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (address is null || (currentAddress != null && currentAddress.Equals(address)))
            {
                return currentAddress;
            }

            ParameterDictionary parameters = GetAddressParameters(address, "eventId", eventId);

            return (await ExecuteStoredProc("[dbo].[spSaveEventAddress]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }

        public Task<Address> SaveLodgingAddressAsync(Guid lodgingId, Address address)
        {
            throw new NotImplementedException();
        }

        public async Task<Address> SaveReunionAddressAsync(Guid reunionId, Address address)
        {
            Address currentAddress = await GetReunionAddressAsync(reunionId).
                ConfigureAwait(continueOnCapturedContext: false);

            if(address is null || (currentAddress != null && currentAddress.Equals(address)))
            {
                return currentAddress;
            }

            ParameterDictionary parameters = GetAddressParameters(address, "reunionId", reunionId);

            return (await ExecuteStoredProc("[dbo].[spSaveReunionAddress]", parameters)
                .ConfigureAwait(continueOnCapturedContext: false)).SingleOrDefault();
        }

        #region Helper Methods

        private static ParameterDictionary GetAddressParameters(Address address, string idColumn, Guid id)
        {
            return new ParameterDictionary(new string[]
            {
                "userId", address.ActionUserId,
                idColumn, id.ToString(),
                "description", address.Description,
                "line1", address.Line1,
                "line2", address.Line2,
                "city", address.City,
                "state", address.State,
                "zipcode", address.ZipCode
            });
        }

        #endregion
    }
}
