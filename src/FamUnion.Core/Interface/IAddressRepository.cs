using FamUnion.Core.Model;
using System;

namespace FamUnion.Core.Interface
{
    public interface IAddressRepository
    {
        Address GetAddress(Guid id);

        Address SaveReunionAddress(Guid reunionId, Address address);

        Address SaveEventAddress(Guid eventId, Address address);

        Address SaveFamilyAddress(Guid familyId, Address address);
    }
}
