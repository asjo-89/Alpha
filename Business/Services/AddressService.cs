using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class AddressService(IAddressRepository addressRepository, IMemberUserRepository memberRepository) : IAddressService
{
    private readonly IAddressRepository _addressRepository = addressRepository;
    private readonly IMemberUserRepository _memberRepository = memberRepository;



    public async Task<AddressResult<Address>> CreateAsync(string streetName, string postalCode, string city, Guid id)
    {
        if (streetName == null && postalCode == null && city == null)
            return new AddressResult<Address> { Succeeded = false, StatusCode = 400, ErrorMessage = "All required fields must be completed." };
                
        try
        {
            var started = await _addressRepository.BeginTransactionAsync();

            //var member = await _memberRepository.GetAsync(x => x.Id == id);
            var member = await _memberRepository.GetAddressAsync(id);
            if (member == null)
                return new AddressResult<Address> { Succeeded = false, StatusCode = 404, ErrorMessage = "Unable to find member when trying to add address." };


            AddressEntity address = new AddressEntity
            {
                StreetAddress = streetName ?? "",
                PostalCode = postalCode ?? "",
                City = city,
                Member = member
            };

            var result = await _addressRepository.CreateAsync(address);

            if (!result.Success)
                return new AddressResult<Address> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to save address." };

            await _addressRepository.CommitTransactionAsync();

            return new AddressResult<Address> { Succeeded = true, StatusCode = 201, Data = address.MapTo<Address>() };
        }
        catch (Exception ex)
        {
            var rollback = await _addressRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new AddressResult<Address> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to save address: {ex.Message} " };
        }
    }
}
