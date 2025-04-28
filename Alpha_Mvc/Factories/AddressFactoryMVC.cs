﻿using Data.Entities;
using Domain.Models;

namespace Data.Factories;

public static class AddressFactoryMVC
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
