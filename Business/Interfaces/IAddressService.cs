using Business.Models;

namespace Business.Interfaces
{
    public interface IAddressService
    {
        Task<AddressResult> CreateAddressAsync(string streetName, string postalCode, string city);
    }
}