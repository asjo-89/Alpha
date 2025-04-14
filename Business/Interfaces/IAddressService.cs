using Business.Models;
using Data.Entities;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IAddressService
    {
        Task<AddressResult<Address>> CreateAsync(string streetName, string postalCode, string city, Guid id);
    }
}