using System;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Request
{
    public class GetEntityAddressRequest
    {
        public EntityType EntityType { get; set; }
        public Guid EntityId { get; set; }

        public GetEntityAddressRequest()
        {

        }

        public GetEntityAddressRequest(EntityType entityType, Guid entityId)
        {
            EntityId = entityId;
            EntityType = entityType;
        }
    }

    public class GetReunionAddressRequest : GetEntityAddressRequest
    {
        public GetReunionAddressRequest()
        {
            EntityType = EntityType.Reunion;
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
            EntityType = EntityType.Event;
        }

        public GetEventAddressRequest(Guid entityId) : this()
        {
            EntityId = entityId;
        }
    }
}
