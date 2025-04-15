using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class AddressFactory
{
    public static AddressEntity CreateEntityFromModel(Address model)
    {
        return new AddressEntity
        {
            StreetAddress = model.StreetAddress,
            PostalCode = model.PostalCode,
            City = model.City
        };
    }

    public static Address CreateModelFromEntity(AddressEntity entity)
    {
        return new Address
        {
            Id = entity.Id,
            StreetAddress = entity.StreetAddress,
            PostalCode = entity.PostalCode,
            City = entity.City
        };
    }
}
