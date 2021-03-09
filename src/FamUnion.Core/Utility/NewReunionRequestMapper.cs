using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using System;

namespace FamUnion.Core.Utility
{
    public class NewReunionRequestMapper : RequestMapperBase<Reunion, NewReunionRequest>
    {
        public static new Reunion Map(NewReunionRequest request)
        {
            var requestLocation = request.Location;
            if (requestLocation != null)
            {
                requestLocation.AddressType = Constants.AddressEntityType.Reunion;
            }

            return new Reunion
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Location = requestLocation
            };
        }
    }

    public static class RequestMappers
    {
        public static T Map<T,C>(C request)
        {
            // Load implementations of RequestMapperBase
            // Search implementations for T/C type combination
            // If found, return result of map function
            // If not found, log error and return null

            throw new NotImplementedException();
        }
    }
}
