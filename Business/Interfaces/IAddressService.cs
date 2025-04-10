using Business.Models;

namespace Business.Interfaces
{
    public interface IAddressService
    {
        Task<AddressResult> CreateAsync(string streetName, string postalCode, string city);
    }
}