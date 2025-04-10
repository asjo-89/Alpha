using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class AddressService(IAddressRepository addressRepository) : IAddressService
{
    private readonly IAddressRepository _addressRepository = addressRepository;


    public async Task<AddressResult> CreateAsync(string streetName, string postalCode, string city)
    {
        if (streetName == null && postalCode == null && city == null)
            return new AddressResult { Succeeded = false, StatusCode = 400, ErrorMessage = "All required fields must be completed." };

        var exists = await _addressRepository.ExistsAsync(x => x.StreetAddress == streetName);
        if (exists.Success)
            return new AddressResult { Succeeded = false, StatusCode = 409, ErrorMessage = "Address already exists." };

        try
        {
            var started = await _addressRepository.BeginTransactionAsync();

            AddressEntity address = new AddressEntity
            {
                StreetAddress = streetName ?? "",
                PostalCode = postalCode ?? "",
                City = city
            };

            var result = await _addressRepository.CreateAsync(address);

            if (!result.Success)
                return new AddressResult { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to save address." };

            await _addressRepository.CommitTransactionAsync();

            return new AddressResult { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            var rollback = await _addressRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new AddressResult { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to save address: {ex.Message} " };
        }
    }
}
