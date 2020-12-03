using FamUnion.Core.Model;
using FamUnion.Core.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamUnion.Core.Interface
{
    public interface IAddressService
    {
        Task<Address> GetAddressByIdAsync(Guid addressId);
        Task<Address> GetEntityAddressAsync(GetEntityAddressRequest request);
        Task<Address> SaveEntityAddressAsync(SaveEntityAddressRequest request);
    }
}
