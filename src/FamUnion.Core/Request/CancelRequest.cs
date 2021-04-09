using System;

namespace FamUnion.Core.Request
{
    public class CancelRequest
    {
        public Guid EntityId { get; set; }
        public string UserId { get; set; }
    }
}
