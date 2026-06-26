using FamUnion.Core.Request;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FamUnion.Infrastructure
{
    public static class TvpHelper
    {
        public static string MapInvites(IEnumerable<InviteRequest> requests)
        {
            var items = requests.Select(r => new
            {
                reunion_id = r.ReunionId,
                email      = r.Email,
                name       = r.Name
            });
            return JsonConvert.SerializeObject(items);
        }
    }
}
