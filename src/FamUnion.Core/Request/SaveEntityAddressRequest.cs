using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Request
{
    public class SaveEntityAddressRequest
    {
        public AddressEntityType EntityType { get; set; }
        public Guid EntityId { get; set; }
        public Address Address { get; set; }
    }

    public class SaveReunionAddressRequest : SaveEntityAddressRequest
    {
        public SaveReunionAddressRequest()
        {
            EntityType = AddressEntityType.Reunion;
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
            EntityType = AddressEntityType.Event;
        }

        public SaveEventAddressRequest(Guid entityId, Address address) : this()
        {
            EntityId = entityId;
            Address = address;
        }
    }
}
