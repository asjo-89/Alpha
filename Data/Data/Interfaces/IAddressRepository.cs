using Data.Entities;

namespace Data.Interfaces;

public interface IAddressRepository
{
    Task<AddressEntity> GetOrAddAsync(string streetAddress, int postalCode, string city);
}
