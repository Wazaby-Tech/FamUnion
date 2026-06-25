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
            const string sql = "SELECT * FROM sp_get_address_by_entity_type_and_id(@entityTypeId, @entityId)";
            ParameterDictionary parameters = new ParameterDictionary(
                "entityTypeId", (int)EntityType.Reunion,
                "entityId",     id
            );
            return (await ExecuteStoredProc(sql, parameters).ConfigureAwait(false)).SingleOrDefault();
        }

        public async Task<Address> GetEventAddressAsync(Guid eventId)
        {
            const string sql = "SELECT * FROM sp_get_address_by_event_id(@eventId)";
            return (await ExecuteStoredProc(sql, ParameterDictionary.Single("eventId", eventId))
                .ConfigureAwait(false)).SingleOrDefault();
        }

        public Task<Address> GetLodgingAddressAsync(Guid lodgingId)
        {
            throw new NotImplementedException();
        }

        public async Task<Address> GetReunionAddressAsync(Guid reunionId)
        {
            const string sql = "SELECT * FROM sp_get_address_by_entity_type_and_id(@entityTypeId, @entityId)";
            ParameterDictionary parameters = new ParameterDictionary(
                "entityTypeId", (int)EntityType.Reunion,
                "entityId",     reunionId
            );
            return (await ExecuteStoredProc(sql, parameters).ConfigureAwait(false)).SingleOrDefault();
        }

        public async Task<Address> SaveEventAddressAsync(Guid eventId, Address address)
        {
            Address currentAddress = await GetEventAddressAsync(eventId).ConfigureAwait(false);

            if (address is null || (currentAddress != null && currentAddress.Equals(address)))
            {
                return currentAddress;
            }

            const string sql = "SELECT * FROM sp_save_event_address(@userId, @eventId, @description, @line1, @line2, @city, @state, @zipcode)";
            ParameterDictionary parameters = GetAddressParameters(address, "eventId", eventId);
            return (await ExecuteStoredProc(sql, parameters).ConfigureAwait(false)).SingleOrDefault();
        }

        public Task<Address> SaveLodgingAddressAsync(Guid lodgingId, Address address)
        {
            throw new NotImplementedException();
        }

        public async Task<Address> SaveReunionAddressAsync(Guid reunionId, Address address)
        {
            Address currentAddress = await GetReunionAddressAsync(reunionId).ConfigureAwait(false);

            if (address is null || (currentAddress != null && currentAddress.Equals(address)))
            {
                return currentAddress;
            }

            const string sql = "SELECT * FROM sp_save_reunion_address(@userId, @reunionId, @description, @line1, @line2, @city, @state, @zipcode)";
            ParameterDictionary parameters = GetAddressParameters(address, "reunionId", reunionId);
            return (await ExecuteStoredProc(sql, parameters).ConfigureAwait(false)).SingleOrDefault();
        }

        #region Helper Methods

        private static ParameterDictionary GetAddressParameters(Address address, string idColumn, Guid id)
        {
            return new ParameterDictionary(
                "userId",      address.ActionUserId,
                idColumn,      id,
                "description", address.Description,
                "line1",       address.Line1,
                "line2",       address.Line2,
                "city",        address.City,
                "state",       address.State,
                "zipcode",     address.ZipCode
            );
        }

        #endregion
    }
}
