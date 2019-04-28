using FamUnion.Core.Model;
using System;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressAsync(Guid id);

        Task<Address> GetReunionAddressAsync(Guid reunionId);

        Task<Address> GetEventAddressAsync(Guid eventId);

        Task<Address> GetFamilyAddressAsync(Guid familyId);

        Task<Address> SaveReunionAddressAsync(Guid reunionId, Address address);

        Task<Address> SaveEventAddressAsync(Guid eventId, Address address);

        Task<Address> SaveFamilyAddressAsync(Guid familyId, Address address);
    }
}
