using System;
using System.Collections.Generic;
using System.Text;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Request
{
    public class GetEntityAddressRequest
    {
        public AddressEntityType EntityType { get; set; }
        public Guid EntityId { get; set; }

        public GetEntityAddressRequest()
        {

        }

        public GetEntityAddressRequest(AddressEntityType entityType, Guid entityId)
        {
            EntityId = entityId;
            EntityType = entityType;
        }
    }

    public class GetReunionAddressRequest : GetEntityAddressRequest
    {
        public GetReunionAddressRequest()
        {
            EntityType = AddressEntityType.Reunion;
        }

        public GetReunionAddressRequest(Guid entityId) : this()
        {
            EntityId = entityId;
        }
    }

    public class GetEventAddressRequest : GetEntityAddressRequest
    {
        public GetEventAddressRequest() 
        {
            EntityType = AddressEntityType.Event;
        }

        public GetEventAddressRequest(Guid entityId) : this()
        {
            EntityId = entityId;
        }
    }
}
