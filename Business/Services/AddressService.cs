using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class AddressService(IAddressRepository addressRepository, IMemberUserRepository memberRepository) : IAddressService
{
    private readonly IAddressRepository _addressRepository = addressRepository;
    private readonly IMemberUserRepository _memberRepository = memberRepository;



    public async Task<AddressResult<Address>> CreateAsync(string streetName, string postalCode, string city)
    {
        if (streetName == null && postalCode == null && city == null)
            return new AddressResult<Address> { Succeeded = false, StatusCode = 400, ErrorMessage = "All required fields must be completed." };
                
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
                return new AddressResult<Address> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to save address." };

            await _addressRepository.CommitTransactionAsync();

            return new AddressResult<Address> { Succeeded = true, StatusCode = 201, Data = AddressFactory.CreateModelFromEntity(address) };
        }
        catch (Exception ex)
        {
            var rollback = await _addressRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new AddressResult<Address> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to save address: {ex.Message} " };
        }
    }
}
