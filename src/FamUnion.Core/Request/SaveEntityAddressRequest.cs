using FamUnion.Core.Model;
using System;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Request
{
    public class SaveEntityAddressRequest
    {
        public EntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Address Address { get; set; }
    }

    public class SaveReunionAddressRequest : SaveEntityAddressRequest
    {
        public SaveReunionAddressRequest()
        {
            EntityType = EntityType.Reunion;
        }

        public SaveReunionAddressRequest(Guid entityId, Address address) : this()
        {
            EntityId = entityId;
            Address = address;
        }
    }

    public class SaveEventAddressRequest : SaveEntityAddressRequest
    {
        public SaveEventAddressRequest()
        {
            EntityType = EntityType.Event;
        }

        public SaveEventAddressRequest(Guid entityId, Address address) : this()
        {
            EntityId = entityId;
            Address = address;
        }
    }
}
