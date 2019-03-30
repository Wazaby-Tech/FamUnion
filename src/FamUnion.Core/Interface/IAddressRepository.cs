using FamUnion.Core.Model;
using System;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IAddressRepository
    {
        Task<Address> GetAddress(Guid id);

        Task<Address> GetReunionAddress(Guid reunionId);

        Task<Address> GetEventAddress(Guid eventId);

        Task<Address> GetFamilyAddress(Guid familyId);

        Task<Address> SaveReunionAddress(Guid reunionId, Address address);

        Task<Address> SaveEventAddress(Guid eventId, Address address);

        Task<Address> SaveFamilyAddress(Guid familyId, Address address);
    }
}
