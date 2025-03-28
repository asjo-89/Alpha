using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class AddressRepository(AlphaDbContext context) : BaseRepository<AddressEntity>(context), IAddressRepository
{
    public async Task<AddressEntity> GetOrAddAsync(string streetAddress, int postalCode, string city)
    {
        AddressEntity? address = await _entities.FirstOrDefaultAsync(x => x.StreetName == streetAddress);
        if (address == null)
        {
            AddressEntity newAddress = new AddressEntity
            {
                StreetName = streetAddress,
                PostalCode = postalCode,
                City = city
            };
            var result = await _context.AddAsync(newAddress);
            return result != null ? newAddress : null!;
        }
        return address;
    }
}
